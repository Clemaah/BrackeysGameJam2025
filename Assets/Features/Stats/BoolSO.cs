using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolSO", menuName = "Boolean", order = 1)]
public class BoolSO : ScriptableObject
{
    [TextArea]
    public string description;
    
    public event Action<bool> OnValueChanged;
    
    public bool baseValue;
    public bool value;

    public void Reset()
    {
        value = baseValue;
    }

    public void ResetEvent()
    {
        OnValueChanged = null;
    }

    private void OnValidate()
    {
        Reset();
    }

    public void ChangeValue(bool newValue)
    {

        if (value == newValue) return;
        
        value = newValue;
        OnValueChanged?.Invoke(value);
    }
}