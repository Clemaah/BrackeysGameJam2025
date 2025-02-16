using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    public float health = 100f;
    [HideInInspector]
    public float maxHealth = 100f;
    
    public bool destroyOnDeath = false;
    
    public UnityEvent onDamage;
    public UnityEvent onDeath;

    private void Awake()
    {
        maxHealth = health;
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
