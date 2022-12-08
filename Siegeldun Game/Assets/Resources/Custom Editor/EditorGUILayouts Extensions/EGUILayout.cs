using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EGUILayout : Editor
{
    protected const string _nullPlaceholder = "NULL";
    public static string nullPlaceholder
    {
        get
        {
            string value;
            try
            {
                value = Globals.nullPlaceholder;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                value = _nullPlaceholder;
            }

            return value;
        }
    }


    // ----------------------------------------------------------- DESIGN FUNCTIONS -----------------------------------------------------------
    public static void SetSpace(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            EditorGUILayout.Space();
        }
    }

    public static void LabelField(string text, bool bold = false)
    {
        if (bold) EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        else EditorGUILayout.LabelField(text);
    }

    public static void IndentLevelRelative(int amount)
    {
        EditorGUI.indentLevel += amount;
    }

    public static void IndentLevelAbsolute(int amount)
    {
        if (amount < 0) return;
        EditorGUI.indentLevel = amount;
    }

    public static string[] StringRepeat(string text, int amount)
    {
        string[] output = new string[amount];
        for (int i = 0; i < amount; i++)
        {
            output[i] = $"{text} {i}";
        }
        return output;
    }

    public static void ShowOnly(dynamic voided)
    {

    }

    public delegate void Del();
    public static void ContentDivider(Del handler, string labelName = "", bool labelBold = true, bool isHeadline = false, bool isEnd = false, bool spaceAtEnd = true)
    {
        if (labelName.Trim() != "") LabelField(labelName, labelBold);

        SetSpace(1);
        IndentLevelRelative(1);
        handler();
        IndentLevelRelative(-1);

        if (spaceAtEnd && !isEnd) SetSpace((isHeadline) ? 3 : 2 );
    }


    // ==================================================== LayerMask ====================================================
    public static string[] LayersList(bool hideBlank = false)
    {
        List<string> layerNames = new List<string>();
        for (int i = 0; i <= 31; i++) //user defined layers start with layer 8 and unity supports 31 layers
        {
            string layer = LayerMask.LayerToName(i);
            if ((hideBlank && layer.Length > 0) || !hideBlank)
            {
                layerNames.Add((layer == "") ? $"Layer {i}" : layer);
            }
        }
        return layerNames.ToArray();
    }

    public static int LayerToMask(string[] options, int value, bool reversed = false)
    {
        List<string> allLayers = new List<string>(LayersList());
        int mask = 0;

        for (int i = 0; i < options.Length; i++) //user defined layers start with layer 8 and unity supports 31 layers
        {
            int idx = allLayers.IndexOf(options[i]);

            int count = (reversed) ? idx : i;
            int increment = (reversed) ? i : idx;

            if ((value & (1 << count)) > 0) // If value is chosen
            {
                mask |= (1 << increment);
            }
        }
        return mask;
    }


    // ----------------------------------------------------------- DEFAULT FUNCTIONS -----------------------------------------------------------
    // ==================================================== Script Name ====================================================
    public static void ScriptName(object target, System.Type type, bool show = true)
    {
        if (!(type == typeof(MonoBehaviour) || type == typeof(ScriptableObject)) || !show) return;

        Object obj = (type == typeof(MonoBehaviour)) ? MonoScript.FromMonoBehaviour((MonoBehaviour)target) : MonoScript.FromScriptableObject((ScriptableObject)target);
        using (new EditorGUI.DisabledScope(true)) EditorGUILayout.ObjectField("Script", obj, type, false);
    }

    public static void ScriptName<T>(object target)
    {
        ScriptName(target, typeof(T));
    }

}
