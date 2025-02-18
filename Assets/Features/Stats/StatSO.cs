using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StatSO", menuName = "Scriptable Objects/Stat")]
public class StatSO : ScriptableObject
{
    [TextArea]
    public string description;
    
    public event Action OnValueChanged;
    public float baseValue;
    public float value;

    public void Reset()
    {
        value = baseValue;
    }

    private void OnValidate()
    {
        Reset();
    }

    public void ChangeValue(float bonus, float multiplier)
    {
        value = (value + bonus) * multiplier;
        OnValueChanged?.Invoke();
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
            FloatValueType.Multiply => value * stat.baseValue,
            _ => 0.0f
        };
    }
}
