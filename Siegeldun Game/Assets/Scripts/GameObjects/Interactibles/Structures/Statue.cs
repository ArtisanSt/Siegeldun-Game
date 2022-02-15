using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : Structures
{
    // ========================================= Structure Initialization =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Statue";
    public override string objectName { get { return _objectName; } }

    private string _structureName = "Statue";
    public override string structureName { get { return _structureName; } }*/

    protected override void Awake()
    {
        base.Awake();
        isInteractible = false;
    }

    protected void Update()
    {
    }

}
