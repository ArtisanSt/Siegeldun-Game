using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EditorBase : Editor, IEditorBase
{
    // ============================== MAIN METHODS ==============================
    public abstract System.Type scriptType { get; }
    protected bool showUneditable = true;

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
    }

    public abstract void BodySettings();

    public virtual void FooterSettings()
    {
        showUneditable = EGUILBase.BoolField(showUneditable, "Show Uneditable", true, true, true);
        if (GUILayout.Button("Save"))
        {
            SaveVariables();
        }
    }


    // ============================== SECONDARY METHODS ==============================
    protected abstract void SetProperty(bool isLeaf);

    protected abstract void SaveVariables();
}
