using System;
using UnityEngine;

public class Damager : MonoBehaviour
{
    
    public float damage;
    
    public bool destroyOnDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable == null) return;
        damageable.TakeDamage(damage);
        if (destroyOnDamage) Destroy(gameObject);
    }
}
