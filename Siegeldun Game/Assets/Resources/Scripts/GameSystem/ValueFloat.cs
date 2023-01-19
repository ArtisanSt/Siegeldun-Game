using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct ValueFloat
{
    public float amount;
    public float percent;

    public float a => amount;
    public float p => percent;

    public float x => amount;
    public float y => percent;

    public ValueFloat(float value, bool isAmount = true)
    {
        amount = 0;
        percent = 0;
        if (isAmount)
            amount = value;
        else
            percent = value;
    }
}

[CustomPropertyDrawer(typeof(ValueFloat))]
public class ValueFloatPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        int indent = EditorGUI.indentLevel;
        float width = position.width / 50;

        Rect labelRect = new Rect(position.x, position.y, 15 * width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, property.displayName);

        EditorGUI.indentLevel++;

        Rect amountRect = new Rect(position.x + 15 * width, position.y, 15 * width, EditorGUIUtility.singleLineHeight);
        Rect percentRect = new Rect(position.x + 30 * width, position.y, 15 * width, EditorGUIUtility.singleLineHeight);
        Rect amountLabelRect = new Rect(amountRect.position.x - 2 * width, position.y, 7 * width, EditorGUIUtility.singleLineHeight);
        Rect percentLabelRect = new Rect(percentRect.position.x - 2 * width, position.y, 7 * width, EditorGUIUtility.singleLineHeight);

        SerializedProperty amount = property.FindPropertyRelative("amount");
        SerializedProperty percent = property.FindPropertyRelative("percent");

        amount.floatValue = EditorGUI.FloatField(amountRect, amount.floatValue);
        percent.floatValue = EditorGUI.FloatField(percentRect, percent.floatValue);
        EditorGUI.LabelField(amountLabelRect, "#");
        EditorGUI.LabelField(percentLabelRect, "%");

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    //T$$anonymous$$s will need to be adjusted based on what you are displaying
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (18 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight);
    }
}
