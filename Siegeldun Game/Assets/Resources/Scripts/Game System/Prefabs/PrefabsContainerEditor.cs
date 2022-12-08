using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabsContainer))]
public class PrefabsContainerEditor : EditorBase
{
    // ============================== MAIN METHODS ==============================
    public override System.Type scriptType { get { return typeof(MonoBehaviour); } }
    protected PrefabsContainer prefabsContainer;
    protected Dictionary<string, bool[]> foldouts = new Dictionary<string, bool[]>() { ["prefabsContainer"] = new bool[1], };

    public override void HeaderSettings()
    {
        base.HeaderSettings();

        prefabsContainer = (PrefabsContainer)target;
    }

    public override void BodySettings()
    {
        EGUILayout.ContentDivider(Div_Main, "PREFABS CONTAINER SETTINGS", true, true, false, true);
    }

    public override void FooterSettings()
    {
        base.FooterSettings();
    }


    // ============================== SECONDARY METHODS ==============================
    protected override void SetProperty(bool isLeaf) { }

    protected override void SaveVariables()
    {
        EditorUtility.SetDirty(prefabsContainer);
    }


    // ============================== TERTIARY METHODS ==============================
    private void Div_Main()
    {
        prefabsContainer.prefabsPath = EGUILBase.StringField(prefabsContainer.prefabsPath, "Prefabs Path:", true, showUneditable);


        /*EGUILBase.StringField(root.objectID, "Object ID:", false, showUneditable);
        EGUILayout.IndentLevelRelative(1);
        List<int> underscores = ProjectUtilities.IndexesOf(root.objectID, "_");
        string objectName = root.objectID.Substring(underscores[0] + 1, underscores[1] - underscores[0] - 1);
        EGUILBase.StringField(objectName, "Object Name:", false, showUneditable);
        EGUILBase.IntField(root.instanceID, "Instance ID:", false, showUneditable);
        EGUILayout.IndentLevelRelative(-1);*/
    }

    /*private void Cnt_EntitySettings()
    {

    }*/
}
