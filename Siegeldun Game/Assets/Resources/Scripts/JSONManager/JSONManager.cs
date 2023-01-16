using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JSONManager
{
    public static string ToJson<TJsonable>(this TJsonable value)
        where TJsonable : IJsonable
    {
        return JsonUtility.ToJson(value, true);

    }
}
