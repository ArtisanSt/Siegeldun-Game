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
        string json = jsonData.value.RemoveEndingComma();
        if (removeOuterParentheses)
            json = json.RemoveOuterParentheses();
        Dictionary<string, string> temp = new Dictionary<string, string>();

        //Debug.Log($"{json}");
        int colonIdx = 0, nextIdx = 0;
        string tempValue, instanceName;
        while (json.Contains(":"))
        {
            colonIdx = json.IndexOf(":");
            instanceName = json.Substring(0, colonIdx).RemoveOuterQuotations();

            int parCount = 0;
            bool isData = false;
            for (int i=colonIdx+1; i<json.Length; i++)
            {
                string chr = $"{json[i]}";
                if (chr == "{" || chr == "[")
                {
                    parCount++;
                    if (!isData)
                        isData = true;
                }
                if (chr == "}" || chr == "]") parCount--;

                if ((parCount == 0 && (isData || chr == ",")) || i == json.Length - 1)
                {
                    nextIdx = (isData && i != json.Length - 1 && json[i+1] == ',') ? i+2 : i+1 ;
                    tempValue = json.Substring(colonIdx + 1, nextIdx - (colonIdx + 1)).RemoveEndingComma();
                    temp.Add(instanceName, tempValue);
                    isData = false;
                    parCount = 0;
                    break;
                }
            }

            if (nextIdx < json.Length)
            {
                json = json.Substring(nextIdx, json.Length - nextIdx).Trim();
            }
            else break;
        }
        return temp;
    }

    public static string RemoveEndingComma(this string value)
    {
        value = value.Trim();
        if (!value.EndsWith(",")) return value;
        return value.Remove(value.Length - 1).Trim();
    }

    public static string RemoveOuterParentheses(this string value)
    {
        value = value.Trim();
        if (!(value.StartsWith("{") && value.EndsWith("}"))) return value;
        return value.Substring(1, value.Length - 2).RemoveEndingComma();
    }

    public static string RemoveOuterQuotations(this string value)
    {
        value = value.Trim();
        if (!(value.StartsWith("\"") && value.EndsWith("\""))) return value;
        return value.Substring(1, value.Length - 2).RemoveEndingComma();
    }

    public static string RemoveOuterParentheses(this JsonData jsonData)
    {
        return jsonData.value.RemoveOuterParentheses();
    }

    public static string CombineData(Dictionary<string, string> allData)
    {
        string data = "";
        int count = 0;
        foreach (KeyValuePair<string, string> eachData in allData)
        {
            data += $"\"{eachData.Key}\" :{((eachData.Value.Contains("{")) ? "\t" : " " )}{eachData.Value}";
            count++;
            if (allData.Count != count)
                data += ",\n";
        }
        return $"{{\n\t{data}\n}}";
    }

    public static JsonData GetJsonData(string jsonPath)
    {
        jsonPath = jsonPath.Replace("\\", "/");
        int startIdx = jsonPath.LastIndexOf("/")+1;
        string instanceName = jsonPath.Substring(startIdx, jsonPath.IndexOf(".json") - startIdx);
        string json = File.ReadAllText(jsonPath);
        return new JsonData(instanceName, json);
    }


    public static void CreateJson(string jsonPath)
    {
        if (File.Exists(jsonPath)) return;
        File.WriteAllText(jsonPath, "{\n}");
    }

    public static void DeleteJson(string jsonPath)
    {
        if (!File.Exists(jsonPath)) return;
        File.Delete(jsonPath);
    }


    public static void SaveJsonData(this JsonData jsonData, string jsonPath)
    {
        File.WriteAllText(jsonPath, jsonData.value);
        Debug.Log($"Data Saved at: {jsonPath}");
    }

    public static JsonData LoadJsonData(this JsonData jsonData, string jsonPath)
    {
        JsonData allData = GetJsonData(jsonPath);
        if (!allData.toDict.ContainsKey(jsonData.key))
        {
            Debug.Log($"Data not found from: {jsonPath}");
            return null;
        }

        Debug.Log($"Data Loaded from: {jsonPath}");
        return new JsonData(jsonData.key, allData.toDict[jsonData.key]);
    }

    public static void DeleteJsonData(this JsonData jsonData, string jsonPath)
    {
        JsonData allData = GetJsonData(jsonPath);
        allData.Remove(jsonData.key);
        File.WriteAllText(jsonPath, allData.value);

        Debug.Log($"Data Deleted From: {jsonPath}");
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
