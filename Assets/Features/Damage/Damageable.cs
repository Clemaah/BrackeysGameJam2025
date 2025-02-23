using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Random = UnityEngine.Random;

public class Damageable : MonoBehaviour
{
    private AudioSource _audioSource;

    public FloatValue health;
    public FloatValue maxHealth;
    public FloatValue armor;
    public FloatValue healthRegen;
    public float invulCooldown = 0;

    public bool destroyOnDeath = false;

    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    public Material redMaterial;

    [SerializeField] public ParticleSystem damageParticles;

    private ParticleSystem _damageParticlesInstance;
    private float _nextHealthChange;
    private bool _isInvulnerable = false;
    private SkinnedMeshRenderer[] _renderers;
    private Material[] _originalMaterials;
    private float _lastTimeRegen;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        maxHealth.stat.OnValueChanged += f => ChangeHealthBy(f, Quaternion.identity);

        if (health.type == FloatValue.FloatValueType.Stat) return;

        health.Set(maxHealth.Get());
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isEditor) return;

        health.Set(maxHealth.Get());
    }
#endif

    private void Update()
    {
        if (healthRegen.Get() == 0) return;
        
        if (_lastTimeRegen > Time.time - 1) return;
        
        _lastTimeRegen = Time.time;
        ChangeHealthBy(healthRegen.Get(), Quaternion.identity, false);
    }

    public void ChangeHealthBy(float amount, Quaternion direction, bool triggerInvulnerability = true)
    {
        if (_isInvulnerable) return;
        float updatedHealth = health.Get();
        updatedHealth += (amount > 0) ? amount : amount * (1 - armor.Get());

        updatedHealth = Mathf.Clamp(updatedHealth, 0.0f, maxHealth.Get());
        float healthChange = updatedHealth - health.Get();
        health.Set(updatedHealth);

        if (Mathf.Abs(healthChange) < 0.0001f) return;
        onHealthChanged.Invoke(healthChange);

        if (healthChange < -0.1f)
        {
            SpawnDamageParticles(direction);
            StartCoroutine(ChangeTexture());
            if (triggerInvulnerability && invulCooldown > 0) StartCoroutine(BecomeInvulnerable());

            if (_audioSource)
            {
                _audioSource.pitch = Random.Range(0.8f, 1.2f);
                _audioSource.volume = 0.25f - healthChange * 0.04f;
                _audioSource.Play();
            }
        }

        // Death
        if (updatedHealth > 0) return;
        onDeath.Invoke();
        if (destroyOnDeath)
            Destroy(gameObject);
    }

    public void SpawnDamageParticles(Quaternion direction)
    {
        _damageParticlesInstance = Instantiate(damageParticles, transform.position - transform.forward, direction);
    }

    public IEnumerator BecomeInvulnerable()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(invulCooldown);
        _isInvulnerable = false;
    }

    private IEnumerator ChangeTexture() {
        _originalMaterials = new Material[_renderers.Length];
        
        for (int i = 0; i < _renderers.Length; i++) {
            _originalMaterials[i] = _renderers[i].material;
            _renderers[i].material = redMaterial;
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < _renderers.Length; i++) {
            _renderers[i].material = _originalMaterials[i];
        }
    }
}
