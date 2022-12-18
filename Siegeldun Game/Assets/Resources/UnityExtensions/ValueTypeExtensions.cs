using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueTypeExtensions
{

}

public static class BinExtensions
{
    public static int ToInt(this int bin)
    {
        int i = 0;
        while (bin != 1)
        {
            bin /= 2;
            i++;
        }
        return i;
    }

}

public static class BoolExtensions
{
    public static int ToInt(this bool value)
    {
        return (value) ? 1 : 0;
    }

    public static bool Evaluate(this bool[] values, params string[] conditions)
    {
        if (values.Length == 0) return false;
        if (conditions.Length == 0)
            conditions = new string[1] { "&&" };

        string[] operators = new string[] { "&&", "||" };
        if (!operators.Contains<string>(conditions).EvaluateAnd())
            throw new System.ArgumentException("Conditions can only be && or ||.", nameof(conditions));

        if (values.Length == 1) return values[0];

        bool output = values[0];
        int opCount = 0;
        if (conditions.Length > 1)
        {
            for (int i = 1; i < values.Length; i++)
            {
                opCount = (i < conditions.Length) ? i : opCount;

                output = (conditions[opCount] == "&&") ? output && values[i] : output || values[i];
            }
        }

        return output;
    }

    public static bool EvaluateAnd(this bool[] values)
    {
        bool arr = values[0];
        for (int i = 1; i < values.Length; i++)
        {
            arr &= values[i];
        }
        return arr;
    }

    public static bool EvaluateOr(this bool[] values)
    {
        bool arr = values[0];
        for (int i = 1; i < values.Length; i++)
        {
            arr |= values[i];
        }
        return arr;
    }
}

public static class IntExtensions
{
    public static int Clamp(this int curValue, int minValue, int maxValue, int addToCurValue = 0)
    {
        return Mathf.Clamp(curValue + addToCurValue, minValue, maxValue);
    }

    public static int Min(this int curValue, int minValue, int addToCurValue = 0)
    {
        return Mathf.Min(curValue + addToCurValue, minValue);
    }

    public static int Max(this int curValue, int maxValue, int addToCurValue = 0)
    {
        return Mathf.Max(curValue + addToCurValue, maxValue);
    }

    public static int ToBin(this int value)
    {
        return int.Parse(System.Convert.ToString(value, 2));
    }
}

public static class FloatExtensions
{
    public static float Clamp(this float curValue, float minValue, float maxValue, float addToCurValue = 0)
    {
        return Mathf.Clamp(curValue + addToCurValue, minValue, maxValue);
    }

    public static float Min(this float curValue, float minValue, float addToCurValue = 0)
    {
        return Mathf.Min(curValue + addToCurValue, minValue);
    }

    public static float Max(this float curValue, float maxValue, float addToCurValue = 0)
    {
        return Mathf.Max(curValue + addToCurValue, maxValue);
    }

    public static bool InRange(this float curValue, float minValue, float maxValue)
    {
        return minValue <= curValue && curValue <= maxValue;
    }
}

public static class StringExtensions
{
    public static string NameToTitle(this string fieldName)
    {
        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

        return r.Replace(fieldName, " ");
    }

    public static string TypeToTitle(this System.Type type)
    {
        return type.Name.NameToTitle();
    }

    public static string FieldToTitle(this System.Reflection.FieldInfo fieldInfo)
    {
        return fieldInfo.Name.NameToTitle();
    }
}

public static class ArrayExtensions
{
    public static List<T> ToList<T>(this T[] arr)
    {
        return System.Linq.Enumerable.ToList<T>(arr);
    }

    public static bool[] Contains<T>(this T[] target, params T[] values) where T : notnull
    {
        bool[] output = new bool[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            output[i] = System.Array.IndexOf(target, values[i]) != -1;
        }
        return output;
    }
}

public static class ListExtensions
{
    public static List<T> AddRange<T>(this List<T> target, params List<T>[] adds) where T: class
    {
        foreach (List<T> add in adds)
        {
            target.AddRange(add);
        }
        return target;
    }
}


