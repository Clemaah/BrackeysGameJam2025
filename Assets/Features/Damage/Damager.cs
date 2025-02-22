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
    public bool destroyOnNonDamageable = false;
    public bool applyDamageOnStay = false;
    public float stayDamageInterval = 0.5f;
    private float _nextDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (!applyDamageOnStay)
            ApplyDamage(other);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!applyDamageOnStay) return;
        if (!enabled) return;
        if (Time.time >= _nextDamage)
        {
            _nextDamage = Time.time + stayDamageInterval;
            ApplyDamage(other);
        }
    }

    private void ApplyDamage(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable == null)
        {
            if (destroyOnNonDamageable)
                Destroy(gameObject);
            return;
        }
        damageable.ChangeHealthBy(-damage.Get(), Quaternion.LookRotation(transform.forward));
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
