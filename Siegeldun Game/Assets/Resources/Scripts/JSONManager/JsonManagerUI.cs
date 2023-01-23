using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Base))]
public class JsonManagerUI : MonoBehaviour
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {
        DestroyComponent();
    }

    // ============================== JSON ==============================
    public Base baseObject => GetComponent<Base>();
    public string jsonPath => baseObject.jsonPath;

    public void DestroyComponent() => Object.DestroyImmediate(this);

    public void SaveData() => baseObject.SaveJsonData();
    public void LoadData() => baseObject.JsonDataToBaseProp();
    public void DeleteData() => baseObject.DeleteJsonData();

    public void CreateJsonFile() => baseObject.CreateJsonFile();
    public void DeleteJsonFile() => baseObject.DeleteJsonFile();
}



[CustomEditor(typeof(JsonManagerUI))]
public class JsonManagerEditor : Editor
{
    private JsonManagerUI root;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        root = (JsonManagerUI)target;

        if (GUILayout.Button("Destroy Component")) root.DestroyComponent();

        EGUILayout.LabelField("DATA SETTINGS", true);
        EGUILayout.SetSpace(1);
        if (GUILayout.Button("Save")) root.SaveData();
        if (GUILayout.Button("Load")) root.LoadData();
        if (GUILayout.Button("Delete")) root.DeleteData();

        EGUILayout.SetSpace(2);
        EGUILayout.LabelField("JSON FILE SETTINGS", true);
        EGUILayout.SetSpace(1);

        if (GUILayout.Button("Create Json")) root.CreateJsonFile();
        if (GUILayout.Button("Recreate Json"))
        {
            root.DeleteJsonFile();
            root.CreateJsonFile();
        }
        if (GUILayout.Button("Delete Json")) root.DeleteJsonFile();
    }
}