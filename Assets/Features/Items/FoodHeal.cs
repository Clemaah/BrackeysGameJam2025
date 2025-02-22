using UnityEngine;

public class FoodHeal : MonoBehaviour
{
    public FloatValue healAmount;
    public BoolSO canBePickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !canBePickedUp.value) return;

        Damageable playerDamageable = other.GetComponent<Damageable>();
        playerDamageable.ChangeHealthBy(healAmount.Get());
        Destroy(gameObject);
    }
}
