using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[Serializable]
public enum ModifierOperation
{
    Set, Mult, Add
}
[System.Serializable]
public struct StatModifier {
    public StatSO statRef;
    
    public ModifierOperation operation;
    public float value;
}

[CreateAssetMenu(fileName = "WishSO", menuName = "Wish", order = 2)]
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
            modifier.statRef.ChangeValue(modifier.operation, modifier.value);
        }
        
        onWishApplied?.Invoke();
        Spawn();
    }

    public void Spawn()
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
        var statRect = new Rect(position.x, position.y, position.width - 130, position.height);
        var operationRect = new Rect(position.x + position.width - 120, position.y, 60, position.height);
        var valueRect = new Rect(position.x + position.width - 50, position.y, 50, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(statRect, property.FindPropertyRelative("statRef"), GUIContent.none);
        EditorGUI.PropertyField(operationRect, property.FindPropertyRelative("operation"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}