using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EGUILBase : EGUILayout
{
    // ----------------------------------------------------------- COMPONENTS FUNCTIONS -----------------------------------------------------------
    // ==================================================== Component ====================================================
    public static T ObjectField<T>(T value, string text = "", bool allowSceneObjects = false, bool editable = true, bool show = true) where T : notnull, UnityEngine.Object
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return (T)EditorGUILayout.ObjectField(text, value, typeof(T), allowSceneObjects);
    }


    // ----------------------------------------------------------- MAIN DATA TYPES FUNCTIONS -----------------------------------------------------------
    // ==================================================== Integer ====================================================
    public static List<int> IntField(List<int> values, string text, bool editable = true, bool show = true)
    {
        return new List<int>(IntField(values.ToArray(), StringRepeat(text, values.Count), editable, show));
    }

    public static List<int> IntField(List<int> values, string[] texts, bool editable = true, bool show = true)
    {
        return new List<int>(IntField(values.ToArray(), texts, editable, show));
    }

    public static int[] IntField(int[] values, string text, bool editable = true, bool show = true)
    {
        return IntField(values, StringRepeat(text, values.Length), editable, show);
    }

    public static int[] IntField(int[] values, string[] texts, bool editable = true, bool show = true)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = IntField(values[i], texts[i], editable, show);
        }
        return values;
    }

    public static int IntField(int value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable))
        {
            if (text.Trim() == "") return (int)EditorGUILayout.IntField(value);
            else return (int)EditorGUILayout.IntField(text, value);
        }
    }

    // ==================================================== Float ====================================================
    public static List<float> FloatField(List<float> values, string text, bool editable = true, bool show = true)
    {
        return new List<float>(FloatField(values.ToArray(), StringRepeat(text, values.Count), editable, show));
    }

    public static List<float> FloatField(List<float> values, string[] texts, bool editable = true, bool show = true)
    {
        return new List<float>(FloatField(values.ToArray(), texts, editable, show));
    }

    public static float[] FloatField(float[] values, string text, bool editable = true, bool show = true)
    {
        return FloatField(values, StringRepeat(text, values.Length), editable, show);
    }

    public static float[] FloatField(float[] values, string[] texts, bool editable = true, bool show = true)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = FloatField(values[i], texts[i], editable, show);
        }
        return values;
    }

    public static float FloatField(float value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable))
        {
            if (text.Trim() == "") return (float)EditorGUILayout.FloatField(value);
            else return (float)EditorGUILayout.FloatField(text, value);
        }
    }

    // ==================================================== Bool ====================================================
    public static List<bool> BoolField(List<bool> values, string text, bool isLeft = false, bool editable = true, bool show = true)
    {
        return new List<bool>(BoolField(values.ToArray(), StringRepeat(text, values.Count), isLeft, editable, show));
    }

    public static List<bool> BoolField(List<bool> values, string[] texts, bool isLeft = false, bool editable = true, bool show = true)
    {
        return new List<bool>(BoolField(values.ToArray(), texts, isLeft, editable, show));
    }

    public static bool[] BoolField(bool[] values, string text, bool isLeft = false, bool editable = true, bool show = true)
    {
        return BoolField(values, StringRepeat(text, values.Length), isLeft, editable, show);
    }

    public static bool[] BoolField(bool[] values, string[] texts, bool isLeft = false, bool editable = true, bool show = true)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = BoolField(values[i], texts[i], isLeft, editable, show);
        }
        return values;
    }

    public static bool BoolField(bool value, string text = "", bool isLeft = false, bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable))
        {
            if (text.Trim() == "") return (bool)EditorGUILayout.Toggle(value);
            else return (isLeft) ? (bool)EditorGUILayout.ToggleLeft(text, value) : (bool)EditorGUILayout.Toggle(text, value);
        }
    }

    // ==================================================== String ====================================================
    public static List<string> StringField(List<string> values, string text, bool editable = true, bool show = true)
    {
        return new List<string>(StringField(values.ToArray(), StringRepeat(text, values.Count), editable, show));
    }

    public static List<string> StringField(List<string> values, string[] texts, bool editable = true, bool show = true)
    {
        return new List<string>(StringField(values.ToArray(), texts, editable, show));
    }

    public static string[] StringField(string[] values, string text, bool editable = true, bool show = true)
    {
        return StringField(values, StringRepeat(text, values.Length), editable, show);
    }

    public static string[] StringField(string[] values, string[] texts, bool editable = true, bool show = true)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = StringField(values[i], texts[i], editable, show);
        }
        return values;
    }

    public static string StringField(string value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable))
        {
            if (text.Trim() == "") return (string)EditorGUILayout.TextField(value);
            else return (string)EditorGUILayout.TextField(text, value);
        }
    }


    // ----------------------------------------------------------- COLLECTION DATA TYPES FUNCTIONS -----------------------------------------------------------
    public static int ListFieldCounter(ref bool state, int amount, string mainText, bool showFoldout = true, bool editable = true, bool show = true)
    {
        if (!editable && !show) return amount;

        EditorGUILayout.BeginHorizontal();
        using (new EditorGUI.DisabledScope(!editable))
        {
            amount = Mathf.Max(0, IntField(amount, mainText));
            SetSpace(1);
            state = (showFoldout) ? EditorGUILayout.Foldout(state, "") : state;
        }
        EditorGUILayout.EndHorizontal();
        return amount;
    }


    // ==================================================== Integer ====================================================
    public static void IntFieldList(ref bool state, ref List<int> values, string mainText, string subTextPrefix, bool editable = true, bool show = true)
    {
        if (!editable && !show) return;

        int amount = ListFieldCounter(ref state, values.Count, mainText, editable, show);

        while (values.Count != amount)
        {
            if (values.Count > amount)
            {
                values.RemoveAt(values.Count - 1);
            }

            if (values.Count < amount)
            {
                values.Add(0);
            }
        }

        if (!state) return;

        IndentLevelRelative(1);
        values = IntField(values, subTextPrefix, editable, show);
        IndentLevelRelative(-1);
    }

    // ==================================================== Float ====================================================
    public static void FloatFieldList(ref bool state, ref List<float> values, string mainText, string subTextPrefix, bool editable = true, bool show = true)
    {
        if (!editable && !show) return;

        int amount = ListFieldCounter(ref state, values.Count, mainText, editable, show);

        while (values.Count != amount)
        {
            if (values.Count > amount)
            {
                values.RemoveAt(values.Count - 1);
            }

            if (values.Count < amount)
            {
                values.Add(0f);
            }
        }

        if (!state) return;

        IndentLevelRelative(1);
        values = FloatField(values, subTextPrefix, editable, show);
        IndentLevelRelative(-1);
    }

    // ==================================================== String ====================================================
    public static void StringFieldList(ref bool state, ref List<string> values, string mainText, string subTextPrefix, bool editable = true, bool show = true)
    {
        if (!editable && !show) return;

        int amount = ListFieldCounter(ref state, values.Count, mainText, editable, show);

        while (values.Count != amount)
        {
            if (values.Count > amount)
            {
                values.RemoveAt(values.Count - 1);
            }

            if (values.Count < amount)
            {
                values.Add("");
            }
        }

        if (!state) return;

        IndentLevelRelative(1);
        values = StringField(values, subTextPrefix, editable, show);
        IndentLevelRelative(-1);
    }

    // ==================================================== Bool ====================================================
    public static void BoolFieldList(ref bool state, ref List<bool> values, string mainText, string subTextPrefix, bool editable = true, bool show = true)
    {
        if (!editable && !show) return;

        int amount = ListFieldCounter(ref state, values.Count, mainText, editable, show);

        while (values.Count != amount)
        {
            if (values.Count > amount)
            {
                values.RemoveAt(values.Count - 1);
            }

            if (values.Count < amount)
            {
                values.Add(false);
            }
        }

        if (!state) return;

        IndentLevelRelative(1);
        values = BoolField(values, subTextPrefix, editable, show);
        IndentLevelRelative(-1);
    }


    // ----------------------------------------------------------- UNITY DATA TYPES FUNCTIONS -----------------------------------------------------------
    // ==================================================== Vector ====================================================
    public static Vector2 Vector2Field(Vector2 value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return EditorGUILayout.Vector2Field(text, value);
    }

    public static Vector2Int Vector2Field(Vector2Int value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return EditorGUILayout.Vector2IntField(text, value);
    }

    public static Vector3 Vector3Field(Vector3 value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return EditorGUILayout.Vector3Field(text, value);
    }

    public static Vector3Int Vector3Field(Vector3Int value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return EditorGUILayout.Vector3IntField(text, value);
    }

    // ==================================================== LayerMask ====================================================
    public static LayerMask LayerMaskField(LayerMask value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        string[] options = LayersList(true);

        using (new EditorGUI.DisabledScope(!editable))
        {
            value.value = LayerToMask(options, value.value, true);
            value.value = EditorGUILayout.MaskField(text, value.value, LayersList(true));
            return LayerToMask(options, value.value);
        }
    }


    // ----------------------------------------------------------- ENUM FUNCTIONS -----------------------------------------------------------
    // ==================================================== Enum ====================================================
    public static System.Enum EnumField(System.Enum value, string text = "", bool editable = true, bool show = true)
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return (System.Enum)EditorGUILayout.EnumPopup(text, value);
    }

    public static T EnumField<T>(T value, string text = "", bool editable = true, bool show = true) where T: System.Enum
    {
        if (!editable && !show) return value;

        using (new EditorGUI.DisabledScope(!editable)) return (T)EditorGUILayout.EnumPopup(text, value);
    }


    // ----------------------------------------------------------- FOLDOUT FUNCTIONS -----------------------------------------------------------
    // ==================================================== Enum ====================================================
    public static bool Foldout(ref bool foldout, bool foldoutEditable, string text, Del handler, bool editable = true, bool show = true) // foldoutProp = foldout, foldoutEditable
    {
        if (!editable && !show) return foldout;

        EditorGUILayout.BeginHorizontal();
        foldoutEditable = BoolField(foldoutEditable, text, editable, show);
        foldout = EditorGUILayout.Foldout(foldout, "");
        EditorGUILayout.EndHorizontal();
        Foldout(foldout, handler, editable, show);
        return foldoutEditable;
    }

    public static bool Foldout(bool foldout, string text, Del handler, bool editable = true, bool show = true)
    {
        if (!editable && !show) return foldout;

        foldout = EditorGUILayout.Foldout(foldout, text);
        Foldout(foldout, handler, editable, show);
        return foldout;
    }

    public static void Foldout(bool foldout, Del handler, bool editable = true, bool show = true)
    {
        if (!foldout || (!editable && !show)) return;

        EGUILayout.IndentLevelRelative(1);
        handler();
        EGUILayout.IndentLevelRelative(-1);
    }

    public static bool Foldout(bool foldout, string text = "", bool isBold = false, bool editable = true, bool show = true)
    {
        EditorGUILayout.BeginHorizontal();
        LabelField(text, isBold);
        foldout = EditorGUILayout.Foldout(foldout, "");
        EditorGUILayout.EndHorizontal();

        return foldout;
    }
}
