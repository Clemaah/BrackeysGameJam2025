using System;
using TMPro;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private WeaponTrigger[] _weapons;

    private void Start()
    {
        _weapons = FindObjectsByType<WeaponTrigger>(FindObjectsSortMode.None);
        
        foreach (var weapon in _weapons)
            weapon.OnPickUp += PickWeapon;
    }
    private void PickWeapon(WeaponTrigger pickedUpWeapon)
    {
        foreach (WeaponTrigger weapon in _weapons)
            weapon.SetActive(pickedUpWeapon != weapon);

        RoomBehaviour roomBehaviour = GetComponent<RoomBehaviour>();
        roomBehaviour.OpenRoom();

        GameManager.Instance.ResetStats();
        pickedUpWeapon.weaponData.Apply();
    }
}
