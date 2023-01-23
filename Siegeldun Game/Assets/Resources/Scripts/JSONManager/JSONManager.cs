using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonManager
{
    // ============================================== IDataProp ==============================================
    public static string ToJson<TDataProp>(this TDataProp value)
        where TDataProp : IDataProp
    {
        return JsonUtility.ToJson(value, true);

    }


    // ============================================== JSONDATA EXCLUSIVE METHODS ==============================================
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


    // ============================================== SCRIPTING METHODS ==============================================
    // JSON FILE:
    public static void CreateJsonFile(string jsonPath) { if (!File.Exists(jsonPath)) File.WriteAllText(jsonPath, "{\n}"); }
    public static void DeleteJsonFile(string jsonPath) { if (File.Exists(jsonPath)) File.Delete(jsonPath); }
    public static JsonData ReadJsonDataFile(string jsonPath)
    {
        CreateJsonFile(jsonPath);
        jsonPath = jsonPath.Replace("\\", "/");
        int startIdx = jsonPath.LastIndexOf("/") + 1;
        string instanceName = jsonPath.Substring(startIdx, jsonPath.IndexOf(".json") - startIdx);
        string json = File.ReadAllText(jsonPath);
        return new JsonData(instanceName, json);
    }

    // JSON DATA ALTERATIONS:

    public static void SaveJsonData(this JsonData jsonData, string jsonPath)
    {
        JsonData allData = ReadJsonDataFile(jsonPath);
        allData.Set(jsonData.key, jsonData.value);
        File.WriteAllText(jsonPath, jsonData.value);
        Debug.Log($"Data Saved at: {jsonPath}");
    }

    public static JsonData LoadJsonData(this JsonData jsonData, string jsonPath) => LoadJsonData(jsonData.key, jsonPath);
    public static JsonData LoadJsonData(string instanceName, string jsonPath)
    {
        JsonData allData = ReadJsonDataFile(jsonPath);
        if (!allData.Contains(instanceName))
        {
            Debug.Log($"Data not found from: {jsonPath}");
            return null;
        }

        Debug.Log($"Data Loaded from: {jsonPath}");
        return new JsonData(instanceName, allData.toDict[instanceName]);
    }

    public static void DeleteJsonData(this JsonData jsonData, string jsonPath) => DeleteJsonData(jsonData.key, jsonPath);
    public static void DeleteJsonData(string instanceName, string jsonPath)
    {
        JsonData allData = ReadJsonDataFile(jsonPath);
        allData.Remove(instanceName);
        File.WriteAllText(jsonPath, allData.value);

        Debug.Log($"Data Deleted From: {jsonPath}");
    }

    public static string GetJsonPath(string dataPath, string baseType) => $"{dataPath}/{baseType}.json";


}

public static class JsonDataManagerBaseExtensions
{
    public static void CreateJsonFile(this Base baseObject) { if (!File.Exists(baseObject.jsonPath)) File.WriteAllText(baseObject.jsonPath, "{\n}"); }
    public static void DeleteJsonFile(this Base baseObject) { if (File.Exists(baseObject.jsonPath)) File.Delete(baseObject.jsonPath); }

    public static JsonData BasePropJDToJsonData(this GameObject gameObject) => BasePropJDToJsonData(gameObject.GetComponents<IJsonable>(), gameObject.GetComponent<Base>().instanceName);
    public static JsonData BasePropJDToJsonData(this Base baseObject) => BasePropJDToJsonData(baseObject.gameObject.GetComponents<IJsonable>(), baseObject.instanceName);
    public static JsonData BasePropJDToJsonData(this IJsonable[] iJsonables, string instanceName)
    {
        JsonData temp = new JsonData(instanceName, false);
        for (int i = 0; i < iJsonables.Length; i++)
        {
            JsonData jsonData = iJsonables[i].BasePropToBasePropJD();
            temp.Set(jsonData.key, jsonData.value);
        }
        temp.Update();
        return temp;
    }

    public static void JsonDataToBaseProp(this Base baseObject) => JsonDataToBaseProp(baseObject.gameObject.GetComponents<IJsonable>(), LoadJsonData(baseObject));
    public static void JsonDataToBaseProp(this IJsonable[] iJsonables, JsonData jsonData)
    {
        for (int i = 0; i < iJsonables.Length; i++)
        {
            iJsonables[i].SetBaseProp(jsonData.toDict[iJsonables[i].componentName]);
        }
    }

    public static void SaveJsonData(this Base baseObject)
    {
        IJsonable[] iJsonables = baseObject.gameObject.GetComponents<IJsonable>();
        JsonData jsonData = new JsonData(baseObject.instanceName, BasePropJDToJsonData(iJsonables, baseObject.instanceName).KeyValuePairToString());
        JsonManager.SaveJsonData(jsonData, baseObject.jsonPath);
    }
    public static JsonData LoadJsonData(this Base baseObject) => JsonManager.LoadJsonData(baseObject.instanceName, baseObject.jsonPath);
    public static void DeleteJsonData(this Base baseObject) => JsonManager.DeleteJsonData(baseObject.instanceName, baseObject.jsonPath);
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

    public bool Contains(string key) => toDict.ContainsKey(key.Trim());
    public string Get(string key) => toDict[key.Trim()];

    public void Update()
    {
        value = JsonManager.CombineData(toDict);
    }

    public string KeyValuePairToString()
    {
        return $"{{\n\"{key}\" : {value}\n}}";
    }
}
