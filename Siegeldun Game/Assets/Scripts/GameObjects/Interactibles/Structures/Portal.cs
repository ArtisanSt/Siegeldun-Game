using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Structures
{
    // ========================================= Structure Initialization =========================================
    private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Portal";
    public override string objectName { get { return _objectName; } }

    private string _structureName = "Portal";
    public override string structureName { get { return _structureName; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Update()
    {
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected) return;

        Debug.Log("ENTERING NEXT STAGE");
        //SceneManager.LoadScene("Testing Grounds");
    }
}
