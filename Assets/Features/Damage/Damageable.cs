using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    [HideInInspector]
    public float health;
    public FloatValue maxHealth;
    
    public FloatValue healthChange;
    
    public bool destroyOnDeath = false;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

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
            TakeDamage(-healthChange.Get());
        }
    }

    public void TakeDamage(float damage)
    {
        float preDamageHealth = health;
        health -= damage;
        health = Mathf.Clamp(health, 0.0f, maxHealth.Get());
        float damageTaken = preDamageHealth - health;
        if (Mathf.Abs(damageTaken) < 0.001f) return;
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
}
