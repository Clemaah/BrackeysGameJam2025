using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Damageable damageable;
    private Slider _slider;
    
    void Start()
    {
        damageable.maxHealth.stat.OnValueChanged += _ => UpdateMaxHealth();
        damageable.onHealthChanged.AddListener(_ => UpdateHealth());
        damageable.onDeath.AddListener(() => Destroy(gameObject));
        _slider = GetComponent<Slider>();

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        UpdateMaxHealth();
        _slider.SetValueWithoutNotify(damageable.health.Get());
    }

    private void UpdateMaxHealth()
    {
        _slider.maxValue = damageable.maxHealth.Get();
        _slider.transform.localScale = new Vector3((damageable.maxHealth.Get() / damageable.maxHealth.stat.baseValue), 1, 1);
    }
}
