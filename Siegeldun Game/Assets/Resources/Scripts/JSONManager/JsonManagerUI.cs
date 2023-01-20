using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class JsonManagerUI : MonoBehaviour
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {
        DestroyComponent();
    }

    // ============================== JSON ==============================
    public string baseType => GetComponent<Base>().baseType.ToString();
    public static string gamePath => Application.persistentDataPath;
    public static string dataPath => $"{gamePath}/Data";
    public string jsonPath => $"{dataPath}/{baseType}.json";

    public string instanceName => GetComponent<Base>().instanceName;

    public JsonData jsonData { get; private set; }
    public JsonData InstancePropToJson()
    {
        IJsonable[] iJsonables = GetComponents<IJsonable>();
        JsonData temp = new JsonData(instanceName, false);
        for (int i = 0; i < iJsonables.Length; i++)
        {
            JsonData jsonData = iJsonables[i].BasePropToJson();
            temp.Set(jsonData.key, jsonData.value);
        }
        temp.Update();
        return temp;
    }

    public void JsonDataToDataProp()
    {
        IJsonable[] iJsonables = GetComponents<IJsonable>();
        for (int i = 0; i < iJsonables.Length; i++)
        {
            string componentName = iJsonables[i].BasePropToJson().key;
            iJsonables[i].SetBaseProp(jsonData.toDict[componentName]);
        }
    }

    public void CreateData()
    {
        if (jsonData == null)
            jsonData = new JsonData(baseType);
    }

    public void SaveData()
    {
        CreateData();
        jsonData.Set(instanceName, InstancePropToJson().value);
        jsonData.SaveJsonData(jsonPath);
    }

    public void LoadData()
    {
        CreateData();
        jsonData = jsonData.LoadJsonData(jsonPath);
        JsonDataToDataProp();
    }

    public void DeleteData()
    {
        CreateData();
        jsonData.DeleteJsonData(jsonPath);
    }


    public void DestroyComponent()
    {
        Object.DestroyImmediate(this);
    }
}



[CustomEditor(typeof(JsonManagerUI))]
public class JsonManagerEditor : EditorBase<MonoBehaviour, JsonManagerUI>
{
    /*
        Process:
            HeaderSettings();
            SetProperty(true);
            BodySettings();
            FooterSettings();
                - SaveVariables();
    */

    // Optional
    public override void HeaderSettings()
    {

    }

    // Required
    public override void BodySettings()
    {
        //EGUILayout.ContentDivider(Div_Main, $"JSON Manager ({root.baseType})", true, true, false, true);
    }

    // Optional
    public override void FooterSettings()
    {
        if (GUILayout.Button("Destroy Component")) root.DestroyComponent();

        EGUILayout.LabelField("DATA SETTINGS", true);
        EGUILayout.SetSpace(1);
        if (GUILayout.Button("Save")) root.SaveData();
        if (GUILayout.Button("Load")) root.LoadData();
        if (GUILayout.Button("Delete")) root.DeleteData();

        EGUILayout.SetSpace(2);
        EGUILayout.LabelField("JSON FILE SETTINGS", true);
        EGUILayout.SetSpace(1);

        if (GUILayout.Button("Create Json")) JsonManager.CreateJson(root.jsonPath);
        if (GUILayout.Button("Recreate Json"))
        {
            JsonManager.DeleteJson(root.jsonPath);
            JsonManager.CreateJson(root.jsonPath);
        }
        if (GUILayout.Button("Delete Json")) JsonManager.DeleteJson(root.jsonPath);

        //base.FooterSettings(); // Optional
    }


    // ============================== SECONDARY METHODS ==============================
    // Optional
    /*protected override void SetProperty(bool isLeaf)
    {

    }*/

    // Required
    protected override void SaveVariables()
    {
        //base.SaveVariables(); // Optional
    }


    // ============================== TERTIARY METHODS ==============================
    /*private void Div_Main()
    {

    }*/
}