using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StatSO", menuName = "Stat", order = 1)]
public class StatSO : ScriptableObject
{
    [TextArea]
    public string description;
    
    public event Action<float> OnValueChanged;
    [Header("Range")]
    public float min;
    public float max = 100;
    
    public float baseValue;
    public float value;

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
        min = Mathf.Min(min, max);
        max = Mathf.Max(min, max);
        baseValue = Mathf.Clamp(baseValue, min, max);
        Reset();
    }

    public void ChangeValue(ModifierOperation operation, float modifierValue)
    {
        float initialValue = value;
        switch (operation)
        {
            case ModifierOperation.Set:
                value = modifierValue;
                break;
            case ModifierOperation.Mult:
                value *= modifierValue;
                break;
            case ModifierOperation.Add:
                value += modifierValue;
                break;
        }
        value = Mathf.Clamp(value, min, max);
        float diff = value - initialValue;
        
        OnValueChanged?.Invoke(diff);
    }
}

[Serializable]
public struct FloatValue
{
    public enum FloatValueType
    {
        Flat,
        Stat,
        Multiply
    }
    
    public FloatValueType type;
    public float value;
    public StatSO stat;

    public float Get()
    {
        return type switch
        {
            FloatValueType.Flat => value,
            FloatValueType.Stat => stat.value,
            FloatValueType.Multiply => value * stat.value,
            _ => 0.0f
        };
    }

    public void Set(float newValue)
    {
        switch(type) {
            case FloatValueType.Stat:
                stat.value = newValue;
                break;
            default:
                value = newValue;
                break;
        }
    }
}
