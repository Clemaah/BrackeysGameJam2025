using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    [HideInInspector]
    public float health;
    public FloatValue maxHealth;
    
    public bool destroyOnDeath = false;
    
    public UnityEvent onDamage;
    public UnityEvent onDeath;

    private void Awake()
    {
        health = maxHealth.Get();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        onDamage.Invoke();
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
