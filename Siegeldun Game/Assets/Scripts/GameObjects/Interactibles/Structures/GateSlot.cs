using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GateSlot : Structures
{
    // ========================================= Structure Initialization =========================================
    private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "GateSlot";
    public override string objectName { get { return _objectName; } }

    private string _structureName = "Gate Slot";
    public override string structureName { get { return _structureName; } }

    [SerializeField] GameObject crystalPrefab;
    [SerializeField] Inventory playerInventory;
    public bool slotted = false;

    protected override void Awake()
    {
        base.Awake();
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    protected void Update()
    {

    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected && playerInventory == null) return;

        int inventorySlot = playerInventory.FindItem("Crystal");
        if (inventorySlot != -1)
        {
            Transform crystal = Drop(1, new Vector2(0, 0.1f), crystalPrefab).transform;
            crystal.parent = transform;
            GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
            isInteractible = false;
            slotted = true;

            playerInventory.RemoveFromInventory(inventorySlot);
        }
    }
}
