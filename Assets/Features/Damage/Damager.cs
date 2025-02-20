using System;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public enum KnockBackType
    {
        ProjectileDirection,
        RelativePosition
    }
    
    public FloatValue damage;
    public FloatValue knockback;
    public KnockBackType knockBackType;
    
    public bool destroyOnDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable == null) return;
        damageable.TakeDamage(damage.Get(), true, Quaternion.LookRotation(transform.forward));
        if (destroyOnDamage) Destroy(gameObject);

        // knock-back
        if (Mathf.Abs(knockback.Get()) < 0.01f) return;
        Character character = damageable.GetComponent<Character>();
        if (character == null) return;
        switch (knockBackType)
        {
            case KnockBackType.ProjectileDirection:
                character.Push(transform.forward, knockback.Get());
                break;
            case KnockBackType.RelativePosition:
                character.Push((character.transform.position - transform.position).X0Z().normalized, knockback.Get());
                break;
        }
    }
}
