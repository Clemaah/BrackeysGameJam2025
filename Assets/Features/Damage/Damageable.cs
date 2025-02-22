using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

public class Damageable : MonoBehaviour
{

    public FloatValue health;
    public FloatValue maxHealth;
    public FloatValue armor;
    public FloatValue healthRegen;
    public float invulCooldown = 0;
    
    public bool destroyOnDeath = false;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;
    
    [SerializeField] public ParticleSystem damageParticles;

    private ParticleSystem _damageParticlesInstance;
    private float _nextHealthChange;
    private bool _isInvulnerable = false;

    private void Awake()
    {
        maxHealth.stat.OnValueChanged += f => ChangeHealthBy(f);

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
        ChangeHealthBy(healthRegen.Get() * Time.deltaTime, false);
    }

    public void ChangeHealthBy(float amount, bool triggerInvulnerability = true)
    {
        if (_isInvulnerable) return;

        float updatedHealth = health.Get();
        updatedHealth += (amount > 0) ? amount : amount * (1 - armor.Get());
        
        updatedHealth = Mathf.Clamp(updatedHealth, 0.0f, maxHealth.Get());
        float healthChange = updatedHealth - health.Get();
        health.Set(updatedHealth);
        
        if (Mathf.Abs(healthChange) < 0.0001f) return;
        onHealthChanged.Invoke(healthChange);
        
        if (triggerInvulnerability && healthChange < 0.0f && invulCooldown > 0) StartCoroutine(BecomeInvulnerable());
        
        // Death
        if (updatedHealth > 0) return;
        onDeath.Invoke();
        if (destroyOnDeath)
            Destroy(gameObject);
    }

    public void SpawnDamageParticles(Quaternion direction) {
        _damageParticlesInstance = Instantiate(damageParticles, transform.position - transform.forward, direction);
    }

    public IEnumerator BecomeInvulnerable()
    {
        _isInvulnerable = true; 
        yield return new WaitForSeconds(invulCooldown); 
        _isInvulnerable = false; 
    }
}
