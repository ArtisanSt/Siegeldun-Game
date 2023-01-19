using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonManager
{
    public static string ToJson<TDataProp>(this TDataProp value)
        where TDataProp : IDataProp
    {
        return JsonUtility.ToJson(value, true);

    }

    public static Dictionary<string, string> SeparateDataToDict(this JsonData jsonData, bool removeOuterParentheses = true)
    {
        string json = jsonData.value.Trim().Trim(',').Trim();
        if (removeOuterParentheses)
            json = json.RemoveOuterParentheses();
        Dictionary<string, string> temp = new Dictionary<string, string>();

        string tempValue, instanceName;
        int parCount = 0, startIdx = 0, nameLength = 0, valueStart = 0;
        for (int i = 0; i < json.Length; i++)
        {
            string chr = $"{json[i]}";

            if ((chr == "," || i == json.Length-1) && parCount == 0)
            {
                valueStart = startIdx + 1 + nameLength;
                tempValue = json.Substring(valueStart, i - valueStart);
                instanceName = json.Substring(startIdx, nameLength).Trim().RemoveOuterParentheses();
                temp.Add(instanceName, tempValue);
                startIdx = i + 1;
            }

            if (chr == "{")
            {
                parCount++;
            }
            if (chr == "}") parCount--;
            if (chr == ":")
                nameLength = i - startIdx;
        }
        return temp;
    }

    public static string RemoveOuterParentheses(this string value)
    {
        return value.Trim().Trim(',').Trim().Substring(1, value.Length - 2).Trim().Trim(',').Trim();
    }

    public static string RemoveOuterParentheses(this JsonData jsonData)
    {
        return jsonData.value.RemoveOuterParentheses();
    }

    public static string CombineData(Dictionary<string, string> allData)
    {
        string data = "";
        foreach (KeyValuePair<string, string> eachData in allData)
        {
            data += $"\"{eachData.Key}\" :{((eachData.Value.Contains("{")) ? "\t" : " " )}{eachData.Value},\n";
        }
        data = data.Trim().Trim(',').Trim(); // Removes the last comma
        return $"{{\n\t{data}\n}}";
    }

    public static JsonData JsonToJsonData(string jsonPath)
    {
        jsonPath = jsonPath.Replace("\\", "/");
        int startIdx = jsonPath.LastIndexOf("/");
        string instanceName = jsonPath.Substring(startIdx, jsonPath.IndexOf(".json") - startIdx);
        string json = File.ReadAllText(jsonPath);
        return new JsonData(instanceName, json);
    }
}

public class JsonData
{
    public string key { get; private set; }
    public string value;
    public bool autoUpdate;
    public Dictionary<string, string> toDict { get; private set; }

    public JsonData(string key, bool autoUpdate = true)
    {
        this.key = key.Trim();
        this.autoUpdate = autoUpdate;
        value = "{}";
        toDict = new Dictionary<string, string>();
    }

    public JsonData(string key, string value, bool autoUpdate = true)
    {
        this.key = key.Trim();
        this.autoUpdate = autoUpdate;
        if (value.Trim() == "{}")
        {
            value = value.Trim();
            return;
        }
        
        this.value = value;
        toDict = this.SeparateDataToDict();
    }


    public void Set(string instanceName, string data)
    {
        toDict[instanceName] = data;
        if (autoUpdate) Update();
    }

    public void Remove(string instanceName)
    {
        toDict.Remove(instanceName);
        if (autoUpdate) Update();
    }

    public void Update()
    {
        value = JsonManager.CombineData(toDict);
    }

    public string ConvertToString()
    {
        return $"{{\"{key}\" : {value}}}";
    }
}
