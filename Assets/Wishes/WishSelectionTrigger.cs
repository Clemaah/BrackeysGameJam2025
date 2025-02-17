using System;
using UnityEngine;

public class WishTrigger : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        UIManager.DisplayUI(MenuType.WishesSelection);
    }
}
