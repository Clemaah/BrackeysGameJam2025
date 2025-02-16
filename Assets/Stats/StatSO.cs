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
}