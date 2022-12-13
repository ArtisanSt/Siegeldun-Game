using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSoCSample : EditorBase<ScriptableObject, SoCSample>
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
        base.HeaderSettings(); // Optional

    }

    // Required
    public override void BodySettings()
    {
        EGUILayout.ContentDivider(Div_Main, "PREFABS CONTAINER SETTINGS", true, true, false, true);
    }

    // Optional
    public override void FooterSettings()
    {
        base.FooterSettings(); // Optional
    }


    // ============================== SECONDARY METHODS ==============================
    // Optional
    protected override void SetProperty(bool isLeaf)
    {

    }

    // Required
    protected override void SaveVariables()
    {
        base.SaveVariables(); // Optional
    }


    // ============================== TERTIARY METHODS ==============================
    private void Div_Main()
    {

    }
}
