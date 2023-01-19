using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct AmountFloat
{
    public float min;
    public float max;
    public float value { get; private set; }

    public AmountFloat(float _min, float _max)
    {
        min = _min;
        max = _max;

        value = _min;
    }

    public float Set(float _value)
    {
        value = _value.Clamp(min, max);
        return value;
    }
}



[CustomPropertyDrawer(typeof(AmountFloat))]
public class AmountFloatPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        int indent = EditorGUI.indentLevel;
        float width = position.width / 50;

        Rect labelRect = new Rect(position.x, position.y, 15 * width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, property.displayName);

        EditorGUI.indentLevel++;

        Rect minRect = new Rect(position.x + 15 * width, position.y, 15 * width, EditorGUIUtility.singleLineHeight);
        Rect maxRect = new Rect(position.x + 30 * width, position.y, 15 * width, EditorGUIUtility.singleLineHeight);
        Rect minLabelRect = new Rect(minRect.position.x - 2 * width, position.y, 7 * width, EditorGUIUtility.singleLineHeight);
        Rect maxLabelRect = new Rect(maxRect.position.x - 2 * width, position.y, 7 * width, EditorGUIUtility.singleLineHeight);

        SerializedProperty min = property.FindPropertyRelative("min");
        SerializedProperty max = property.FindPropertyRelative("max");

        min.floatValue = EditorGUI.FloatField(minRect, min.floatValue);
        max.floatValue = EditorGUI.FloatField(maxRect, max.floatValue);
        EditorGUI.LabelField(minLabelRect, "Min");
        EditorGUI.LabelField(maxLabelRect, "Max");

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    //T$$anonymous$$s will need to be adjusted based on what you are displaying
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (18 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight);
    }
}
