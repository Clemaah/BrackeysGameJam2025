using System;
using UnityEngine;

public class WishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        GameManager.UIManager.OpenMenu(MenuType.WishesSelection);
        Destroy(gameObject);
    }
}
