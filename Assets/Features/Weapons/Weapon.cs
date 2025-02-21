using System;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponSO weaponData;
    public TextMeshProUGUI text;

    private void Start()
    {
        text.text = weaponData.weapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        weaponData.Apply();
    }
}
