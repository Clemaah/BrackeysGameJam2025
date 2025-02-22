using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon", order = -1)]
public class WeaponSO : ScriptableObject
{
    [FormerlySerializedAs("modifiers")] [Header("Modifiers")]
    public StatModifier[] stats;
    public BoolModifier[] bools;
    public MaterialModifier[] materials;

    public void Apply()
    {
        foreach (var modifier in stats)
            modifier.statRef.ChangeValue(modifier.operation, modifier.value);
        foreach (var modifier in bools)
            modifier.boolRef.ChangeValue(modifier.value);
        foreach (var modifier in materials)
            modifier.materialRef.ChangeValue(modifier.value);
    }
}