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
    public GameObject objectToSpawnOnNonDamage;
    public bool applyDamageOnStay = false;
    public float stayDamageInterval = 0.5f;
    private float _nextDamage;
    
    private bool _pendingDestroy = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (_pendingDestroy) return;
        if (other.isTrigger) return;
        if (!applyDamageOnStay)
            ApplyDamage(other);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (_pendingDestroy) return;
        if (!applyDamageOnStay) return;
        if (other.isTrigger) return;
        if (!enabled) return;
        if (Time.time >= _nextDamage)
        {
            if(ApplyDamage(other))
                _nextDamage = Time.time + stayDamageInterval;
        }
    }

    private bool ApplyDamage(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable == null)
        {
            if (destroyOnNonDamageable)
            {
                if (objectToSpawnOnNonDamage)
                    Instantiate(objectToSpawnOnNonDamage, transform.position, transform.rotation);
                _pendingDestroy = true;
                Destroy(gameObject);
            }
            return false;
        }
        damageable.ChangeHealthBy(-damage.Get(), Quaternion.LookRotation(transform.forward));
        if (destroyOnDamage) Destroy(gameObject);

        // knock-back
        if (Mathf.Abs(knockback.Get()) < 0.01f) return true;
        Character character = damageable.GetComponent<Character>();
        if (character == null) return true;
        switch (knockBackType)
        {
            case KnockBackType.ProjectileDirection:
                character.Push(transform.forward, knockback.Get());
                break;
            case KnockBackType.RelativePosition:
                character.Push((character.transform.position - transform.position).X0Z().normalized, knockback.Get());
                break;
        }

        return true;
    }
}
