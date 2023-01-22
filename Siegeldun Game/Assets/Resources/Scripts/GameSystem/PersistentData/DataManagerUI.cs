using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class DataManagerUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DestroyComponent();
    }

    private SiegeldunData sgldn;
    public List<Difficulty> difficulties;

    public void Save()
    {
        string[] diffs = new string[difficulties.Count];
        for (int i = 0; i < difficulties.Count; i++)
        {
            diffs[i] = difficulties[i].ToJson();
        }
        sgldn.difficulties = diffs;

        string path = GameSystem.siegeldunDataPath;
        DataManager.CreateSgldnFile<SiegeldunData>(path, sgldn);
    }

    public void Load()
    {
        string path = GameSystem.siegeldunDataPath;
        sgldn = DataManager.LoadSgldnFile<SiegeldunData>(path);
        GameSystem.instance.SetSiegeldunData(sgldn);
    }


    public void DestroyComponent()
    {
        Object.DestroyImmediate(this);
    }
}



[CustomEditor(typeof(DataManagerUI))]
public class DataManagerEditor : Editor
{
    private DataManagerUI root;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        root = (DataManagerUI)target;

        if (GUILayout.Button("Destroy Component")) root.DestroyComponent();

        EGUILayout.LabelField("SIEGELDUN DATA", true);
        EGUILayout.SetSpace(1);
        if (GUILayout.Button("Save")) root.Save();
        if (GUILayout.Button("Load")) root.Load();
        //if (GUILayout.Button("Delete")) root.Load();
    }
}
