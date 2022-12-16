using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// N = ScriptableObject or Monobehavior
// T = Monobehaviour or Scriptable Object Class Instance
public abstract class EditorBase<N, T> : Editor, IEditorBase where N : Object where T: N
{
    // ============================== MAIN METHODS ==============================
    //public abstract System.Type scriptType { get; }
    public System.Type scriptType { get { return typeof(N); } }
    protected bool showUneditable = true;
    protected Dictionary<string, bool[]> foldouts = new Dictionary<string, bool[]>();
    protected T root;

    public override void OnInspectorGUI()
    {
        HeaderSettings();
        SetProperty(true);
        EGUILayout.SetSpace(3);
        BodySettings();
        EGUILayout.SetSpace(3);
        FooterSettings();
    }

    public virtual void HeaderSettings()
    {
        EGUILBase.ScriptName(target, scriptType);
        root = (T)target;
    }

    public abstract void BodySettings();

    public virtual void FooterSettings()
    {
        showUneditable = EGUILBase.BoolField(showUneditable, "Show Uneditable", true, true, true);
        if (GUILayout.Button("Save")) { SaveVariables(); }
    }


    // ============================== SECONDARY METHODS ==============================
    protected virtual void SetProperty(bool isLeaf) { }

    protected virtual void SaveVariables()
    {
        EditorUtility.SetDirty(root);
    }
}
