using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public struct StatModifier {
    public StatSO statRef;
    
    [Tooltip("Bonus modifier")]
    public float bonus;
    
    [Tooltip("Multiplier modifier")]
    public float multiplier;
}

[CreateAssetMenu(fileName = "WishSO", menuName = "Scriptable Objects/Wish")]
public class WishSO : ScriptableObject
{
    [TextArea]
    public string wish;
    [TextArea]
    public string genieCommentary;
    public StatModifier[] modifiers;
    public GameObject[] objectsToSpawn;
    public UnityEvent onWishApplied;

    public void Apply()
    {
        foreach (var modifier in modifiers)
        {
            modifier.statRef.ChangeValue(modifier.bonus, modifier.multiplier);
        }
        
        onWishApplied?.Invoke();
        Spawn();
    }

    private void Spawn()
    {
        foreach (GameObject prefab in objectsToSpawn)
        {
            Instantiate(prefab);
        }
    }
    
}




// ModifierDrawer
[CustomPropertyDrawer(typeof(StatModifier))]
public class StatModifierDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw a foldout header instead of "Element 0", "Element 1"...
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), false, "", false);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var statRect = new Rect(position.x, position.y, position.width - 90, position.height);
        var bonusRect = new Rect(position.x + position.width - 70, position.y, 25, position.height);
        var multiplierRect = new Rect(position.x + position.width - 25, position.y, 25, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(statRect, property.FindPropertyRelative("statRef"), GUIContent.none);
        EditorGUI.PropertyField(bonusRect, property.FindPropertyRelative("bonus"), GUIContent.none);
        EditorGUI.PropertyField(multiplierRect, property.FindPropertyRelative("multiplier"), GUIContent.none);
        
        Rect addRect = new Rect(position.x + position.width - 85, position.y, 10, position.height);
        EditorGUI.LabelField(addRect, "+");
        Rect multRect = new Rect(position.x + position.width - 40, position.y, 10, position.height);
        EditorGUI.LabelField(multRect, "x");

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}