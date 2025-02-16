using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatSO", menuName = "Scriptable Objects/StatSO")]
public class StatSO : ScriptableObject
{
    public float baseValue;
    public float value;
    
    public void Reset()
    {
        value = baseValue;
    }
}
