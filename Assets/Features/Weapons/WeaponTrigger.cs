using System;
using TMPro;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public event Action<WeaponTrigger> OnPickUp;
    public WeaponSO weaponData;
    public TextMeshPro text;

    private void Start()
    {
        text.text = weaponData.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnPickUp?.Invoke(this);
    }

    public void SetActive(bool Activate)
    {
        text.gameObject.SetActive(Activate);
        gameObject.SetActive(Activate);
    }
}
