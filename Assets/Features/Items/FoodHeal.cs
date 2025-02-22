using UnityEngine;

public class FoodHeal : MonoBehaviour
{
    public int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Damageable playerDamageable = other.GetComponent<Damageable>();
        playerDamageable.ChangeHealthBy(healAmount);
        Destroy(gameObject);
    }
}
