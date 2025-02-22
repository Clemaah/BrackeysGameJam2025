using System;
using UnityEngine;

public class FoodHeal : MonoBehaviour
{
    public SphereCollider Collider;
    public FloatValue healAmount;
    public BoolSO canBePickedUp;

    private void Start()
    {
        Collider.isTrigger = canBePickedUp.value;
        canBePickedUp.OnValueChanged += newValue => { Collider.isTrigger = newValue; };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !canBePickedUp.value) return;

        Damageable playerDamageable = other.GetComponent<Damageable>();
        playerDamageable.ChangeHealthBy(healAmount.Get(), Quaternion.identity);
        Destroy(gameObject);
    }
}
