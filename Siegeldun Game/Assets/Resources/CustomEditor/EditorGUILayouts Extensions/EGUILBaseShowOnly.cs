using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EGUILBaseShowOnly : EGUILayout
{
    // ----------------------------------------------------------- COMPONENTS FUNCTIONS -----------------------------------------------------------
    // ==================================================== Component ====================================================
    public static void ObjectField(Object value, System.Type type, string text = "", bool allowSceneObjects = false)
    {
        using (new EditorGUI.DisabledScope(true)) { Object temp = EditorGUILayout.ObjectField(text, value, type, allowSceneObjects); }
    }


    /*// ----------------------------------------------------------- MAIN DATA TYPES FUNCTIONS -----------------------------------------------------------
    // ==================================================== Integer ====================================================
    public static List<int> IntField(List<int> values, string text, bool editable = true)
    {
        return new List<int>(IntField(values.ToArray(), StringRepeat(text, values.Count), editable));
    }

    public static List<int> IntField(List<int> values, string[] texts, bool editable = true)
    {
        return new List<int>(IntField(values.ToArray(), texts, editable));
    }

    public static int[] IntField(int[] values, string text, bool editable = true)
    {
        return IntField(values, StringRepeat(text, values.Length), editable);
    }

    public static int[] IntField(int[] values, string[] texts, bool editable = true)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = IntField(values[i], texts[i], editable);
        }
        return values;
    }

    public static int IntField(int value, string text = "", bool editable = true)
    {
        using (new EditorGUI.DisabledScope(!editable))
        {
            if (text.Trim() == "") return (int)EditorGUILayout.IntField(value);
            else return (int)EditorGUILayout.IntField(text, value);
        }
    }*/

    // ==================================================== Float ====================================================
    public static void FloatField(List<float> values, string text)
    {
        FloatField(values.ToArray(), StringRepeat(text, values.Count));
    }

    public static void FloatField(List<float> values, string[] texts)
    {
        FloatField(values.ToArray(), texts);
    }

    public static void FloatField(float[] values, string text)
    {
        FloatField(values, StringRepeat(text, values.Length));
    }

    public static void FloatField(float[] values, string[] texts)
    {
        for (int i = 0; i < values.Length; i++)
        {
            FloatField(values[i], texts[i]);
        }
    }

    public static void FloatField(float value, string text = "")
    {
        float temp;
        using (new EditorGUI.DisabledScope(true))
        {
            if (text.Trim() == "") { temp = (float)EditorGUILayout.FloatField(value); }
            else { temp = (float)EditorGUILayout.FloatField(text, value); }
        }
    }

    // ==================================================== Bool ====================================================
    public static void BoolField(List<bool> values, string text, bool isLeft = false)
    {
        BoolField(values.ToArray(), StringRepeat(text, values.Count), isLeft);
    }

    public static void BoolField(List<bool> values, string[] texts, bool isLeft = false)
    {
        BoolField(values.ToArray(), texts, isLeft);
    }

    public static void BoolField(bool[] values, string text, bool isLeft = false)
    {
        BoolField(values, StringRepeat(text, values.Length), isLeft);
    }

    public static void BoolField(bool[] values, string[] texts, bool isLeft = false)
    {
        for (int i = 0; i < values.Length; i++)
        {
            BoolField(values[i], texts[i], isLeft);
        }
    }

    public static void BoolField(bool value, string text = "", bool isLeft = false)
    {
        using (new EditorGUI.DisabledScope(true))
        {
            bool temp;
            if (text.Trim() == "") { temp = (bool)EditorGUILayout.Toggle(value); }
            else { temp = (isLeft) ? (bool)EditorGUILayout.ToggleLeft(text, value) : (bool)EditorGUILayout.Toggle(text, value); }
        }
    }

    /*// ==================================================== String ====================================================
    public static List<string> StringField(List<string> values, string text, bool editable = true)
    {
        return new List<string>(StringField(values.ToArray(), StringRepeat(text, values.Count), editable));
    }

    public static List<string> StringField(List<string> values, string[] texts, bool editable = true)
    {
        return new List<string>(StringField(values.ToArray(), texts, editable));
    }

    public static string[] StringField(string[] values, string text, bool editable = true)
    {
        return StringField(values, StringRepeat(text, values.Length), editable);
    }

    public static string[] StringField(string[] values, string[] texts, bool editable = true)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = StringField(values[i], texts[i], editable);
        }
        return values;
    }

    public static string StringField(string value, string text = "", bool editable = true)
    {
        using (new EditorGUI.DisabledScope(!editable))
        {
            if (text.Trim() == "") return (string)EditorGUILayout.TextField(value);
            else return (string)EditorGUILayout.TextField(text, value);
        }
    }*/


    /*// ----------------------------------------------------------- COLLECTION DATA TYPES FUNCTIONS -----------------------------------------------------------
    public static int ListFieldCounter(ref bool state, int amount, string mainText, bool showFoldout = true, bool editable = true)
    {
        EditorGUILayout.BeginHorizontal();
        using (new EditorGUI.DisabledScope(!editable))
        {
            amount = Mathf.Max(0, IntField(amount, mainText));
            SetSpace(1);
            state = (showFoldout) ? EditorGUILayout.Foldout(state, "") : state;
        }
        EditorGUILayout.EndHorizontal();
        return amount;
    }*/


    /*// ==================================================== Integer ====================================================
    public static void IntFieldList(ref bool state, ref List<int> values, string mainText, string subTextPrefix, bool editable = true)
    {
        int amount = ListFieldCounter(ref state, values.Count, mainText, editable);

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
        IntField(ref values, subTextPrefix, editable);
        IndentLevelRelative(-1);
    }*/

    /*// ==================================================== Float ====================================================
    public static void FloatFieldList(ref bool state, ref List<float> values, string mainText, string subTextPrefix, bool editable = true)
    {
        int amount = ListFieldCounter(ref state, values.Count, mainText, editable);

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
        FloatField(ref values, subTextPrefix, editable);
        IndentLevelRelative(-1);
    }*/

    /*// ==================================================== String ====================================================
    public static void StringFieldList(ref bool state, ref List<string> values, string mainText, string subTextPrefix, bool editable = true)
    {
        int amount = ListFieldCounter(ref state, values.Count, mainText, editable);

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
        StringField(ref values, subTextPrefix, editable);
        IndentLevelRelative(-1);
    }*/

    /*// ==================================================== Bool ====================================================
    public static void BoolFieldList(ref bool state, ref List<bool> values, string mainText, string subTextPrefix, bool editable = true)
    {
        int amount = ListFieldCounter(ref state, values.Count, mainText, editable);

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
        BoolField(ref values, subTextPrefix, editable);
        IndentLevelRelative(-1);
    }*/



    // ----------------------------------------------------------- UNITY DATA TYPES FUNCTIONS -----------------------------------------------------------
    public static void Vector2Field(Vector2 value, string text = "")
    {
        using (new EditorGUI.DisabledScope(true)) { Vector2 temp = EditorGUILayout.Vector2Field(text, value); }
    }

    public static void Vector2Field(Vector2Int value, string text = "")
    {
        using (new EditorGUI.DisabledScope(true)) { Vector2Int temp = EditorGUILayout.Vector2IntField(text, value); }
    }

    public static void Vector2Field(Vector3 value, string text = "")
    {
        using (new EditorGUI.DisabledScope(true)) { Vector3 temp = EditorGUILayout.Vector3Field(text, value); }
    }

    public static void Vector2Field(Vector3Int value, string text = "")
    {
        using (new EditorGUI.DisabledScope(true)) { Vector3Int temp = EditorGUILayout.Vector3IntField(text, value); }
    }

    // ==================================================== LayerMask ====================================================
    public static void LayerMaskField(LayerMask value, string text = "")
    {
        string[] options = LayersList(true);

        using (new EditorGUI.DisabledScope(true))
        {
            LayerMask temp = LayerToMask(options, value.value, true);
            temp = EditorGUILayout.MaskField(text, temp, LayersList(true));
            temp = LayerToMask(options, temp);
        }
    }


    // ----------------------------------------------------------- ENUM FUNCTIONS -----------------------------------------------------------
    // ==================================================== ENUM ====================================================
    public static void EnumField(System.Enum value, string text = "")
    {
        using (new EditorGUI.DisabledScope(true)) { System.Enum temp = (System.Enum)EditorGUILayout.EnumPopup(text, value); }
    }


    // ----------------------------------------------------------- OBJECT FUNCTIONS -----------------------------------------------------------
    // ==================================================== GameObject ====================================================
    public static void ObjectField(GameObject value, string text = "", bool allowSceneObjects = false)
    {
        using (new EditorGUI.DisabledScope(true)) { GameObject temp = (GameObject)EditorGUILayout.ObjectField(text, value, typeof(GameObject), allowSceneObjects); }
    }
}
