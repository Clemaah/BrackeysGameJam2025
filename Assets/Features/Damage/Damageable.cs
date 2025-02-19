using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Damageable : MonoBehaviour
{

    [HideInInspector]
    public float health;
    public FloatValue maxHealth;
    
    public FloatValue healthChange;
    
    public bool destroyOnDeath = false;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    [SerializeField] public ParticleSystem _damageParticles;

    private ParticleSystem _damageParticlesInstance;
    private float _nextHealthChange;

    private void Awake()
    {
        health = maxHealth.Get();
    }

    private void Update()
    {
        if (Time.time >= _nextHealthChange)
        {
            _nextHealthChange = Time.time + 1.0f;
            Quaternion nullQuaternion = new Quaternion();
            TakeDamage(-healthChange.Get(), false, nullQuaternion);
        }
    }

    public void TakeDamage(float damage, bool spawnParticles, Quaternion direction)
    {
        float preDamageHealth = health;
        health -= damage;
        health = Mathf.Clamp(health, 0.0f, maxHealth.Get());
        float damageTaken = preDamageHealth - health;
        if (Mathf.Abs(damageTaken) < 0.001f) return;

        if (spawnParticles) SpawnDamageParticles(direction);

        onHealthChanged.Invoke(-damageTaken);
        if (health <= 0)
        {
            onDeath.Invoke();
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
    }

    private void SpawnDamageParticles(Quaternion direction) {
        _damageParticlesInstance = Instantiate(_damageParticles, transform.position - transform.forward, direction);
    }
}
