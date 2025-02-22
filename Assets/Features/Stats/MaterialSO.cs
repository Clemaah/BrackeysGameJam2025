using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSO", menuName = "Material", order = 1)]
public class MaterialSO : ScriptableObject
{
    [TextArea]
    public string description;
    
    public event Action<Material> OnValueChanged;
    
    public Material value;

    public void Reset()
    {
        value = null;
    }

    private void OnValidate()
    {
        Reset();
    }

    public void ResetEvent()
    {
        OnValueChanged = null;
    }

    public void ChangeValue(Material newValue)
    {
        if (!newValue || value == newValue) return;
        
        value = newValue;
        OnValueChanged?.Invoke(value);
    }
}