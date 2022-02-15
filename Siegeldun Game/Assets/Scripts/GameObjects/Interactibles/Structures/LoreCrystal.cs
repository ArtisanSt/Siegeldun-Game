using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreCrystal : Structures
{
    // ========================================= Structure Initialization =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "LoreCrystal";
    public override string objectName { get { return _objectName; } }

    private string _structureName = "Lore Crystal";
    public override string structureName { get { return _structureName; } }*/

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Update()
    {
        if (!GetComponent<LoreDialogue>().loreDialogue.repeatable && GetComponent<LoreDialogue>().loreDialogue.isDone)
        {
            GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
            isInteractible = false;
        }
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected) return;

        GetComponent<LoreDialogue>().StartDialogue();
    }
}
