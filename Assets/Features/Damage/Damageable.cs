using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Damageable : MonoBehaviour
{

    [HideInInspector]
    public float health;
    public FloatValue maxHealth;
    public FloatValue armor;
    public FloatValue healthRegen;
    
    public bool destroyOnDeath = false;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;
    
    [SerializeField] public ParticleSystem damageParticles;

    private ParticleSystem _damageParticlesInstance;
    private float _nextHealthChange;

    private void Awake()
    {
        health = maxHealth.Get();
        
        maxHealth.stat.OnValueChanged += ChangeHealthBy;
    }

    private void Update()
    {
        if (healthRegen.Get() == 0) return;
        ChangeHealthBy(healthRegen.Get() * Time.deltaTime);
    }

    public void ChangeHealthBy(float amount)
    {
        float initialHealth = health;
        health += (amount > 0) ? amount : amount * (1 - armor.Get());
        
        
        health = Mathf.Clamp(health, 0.0f, maxHealth.Get());
        float healthChange = health - initialHealth;
        
        if (Mathf.Abs(healthChange) < 0.001f) return;
        onHealthChanged.Invoke(healthChange);
        
        if (health > 0) return;
            
        onDeath.Invoke();
        
        if (destroyOnDeath)
            Destroy(gameObject);
    }

    public void SpawnDamageParticles(Quaternion direction) {
        _damageParticlesInstance = Instantiate(damageParticles, transform.position - transform.forward, direction);
    }
}
