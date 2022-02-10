using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreCrystal : Structures, IInteractible
{
    // ========================================= Structure Initialization =========================================
    private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "LoreCrystal";
    public override string objectName { get { return _objectName; } }

    private string _structureName = "Lore Crystal";
    public override string structureName { get { return _structureName; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Update()
    {

    }

    // Interaction Event
    public void Interact()
    {
        if (!isSelected) return;

        gameObject.GetComponent<DialogueTrigger>().TiggerDialogue();
    }
}
