using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon", order = -1)]
public class WeaponSO : ScriptableObject
{
    public string weapon;
    public StatModifier[] modifiers;

    public void Apply()
    {
        foreach (var modifier in modifiers)
        {
            modifier.statRef.ChangeValue(modifier.operation, modifier.value);
        }
    }
}