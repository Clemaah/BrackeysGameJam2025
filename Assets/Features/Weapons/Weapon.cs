using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WishSO weaponData;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        weaponData.Apply();
    }
}
