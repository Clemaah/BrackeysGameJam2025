using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
[System.Serializable]
public struct BoolModifier {
    public BoolSO boolRef;
    public bool value;
}
[System.Serializable]
public struct MaterialModifier {
    public MaterialSO materialRef;
    public Material value;
}

[CreateAssetMenu(fileName = "WishSO", menuName = "Wish", order = 2)]
public class WishSO : ScriptableObject
{
    [TextArea]
    public string wish;
    [TextArea]
    public string genieCommentary;
    
    [FormerlySerializedAs("modifiers")] [Header("Modifiers")]
    public StatModifier[] stats;
    public BoolModifier[] bools;
    public MaterialModifier[] materials;
    
    [Header("Events")]
    public Action OnWishApplied;

    public void Apply()
    {
        foreach (var modifier in stats)
            modifier.statRef.ChangeValue(modifier.operation, modifier.value);
        foreach (var modifier in bools)
            modifier.boolRef.ChangeValue(modifier.value);
        foreach (var modifier in materials)
            modifier.materialRef.ChangeValue(modifier.value);
        
        OnWishApplied?.Invoke();
    }
}


// ModifierDrawer
[CustomPropertyDrawer(typeof(StatModifier))]
public class StatModifierDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), false, "", false);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var statRect = new Rect(position.x, position.y, position.width - 130, position.height);
        var operationRect = new Rect(position.x + position.width - 120, position.y, 60, position.height);
        var valueRect = new Rect(position.x + position.width - 50, position.y, 50, position.height);

        EditorGUI.PropertyField(statRect, property.FindPropertyRelative("statRef"), GUIContent.none);
        EditorGUI.PropertyField(operationRect, property.FindPropertyRelative("operation"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}

// ModifierDrawer
[CustomPropertyDrawer(typeof(BoolModifier))]
public class BoolModifierDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), false, "", false);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var boolRect = new Rect(position.x, position.y, position.width - 60, position.height);
        var valueRect = new Rect(position.x + position.width - 50, position.y, 50, position.height);

        EditorGUI.PropertyField(boolRect, property.FindPropertyRelative("boolRef"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

// ModifierDrawer
[CustomPropertyDrawer(typeof(MaterialModifier))]
public class MaterialModifierDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
            
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), false, "", false);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var materialRect = new Rect(position.x, position.y, position.width / 2 - 10, position.height);
        var valueRect = new Rect(position.x + position.width / 2 + 10, position.y, position.width / 2 - 10, position.height);

        EditorGUI.PropertyField(materialRect, property.FindPropertyRelative("materialRef"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}