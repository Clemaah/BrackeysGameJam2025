using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatSO", menuName = "Scriptable Objects/Stat")]
public class StatSO : ScriptableObject
{
    [TextArea]
    public string description;
    
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
