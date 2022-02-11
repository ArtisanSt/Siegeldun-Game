using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GateSlot : Structures, IGateSlot
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

    [Header("GATE SLOT SETTINGS", order = 0)]
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] Inventory playerInventory;
    private bool _slotted = false;
    public bool slotted { get { return _slotted; } protected set { _slotted = value; } }

    protected override void Awake()
    {
        base.Awake();
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        crystalPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/EnvironmentPrefabs/Crystal.prefab");
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
            //Transform crystal = Instantiate(1, new Vector2(0, 0.1f), crystalPrefab, transform).transform;
            Transform crystal = Instantiate(crystalPrefab, new Vector3(transform.position.x, transform.position.y + 0.1f, 0), Quaternion.identity, transform).transform;
            crystal.parent = transform;
            GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
            isInteractible = false;
            slotted = true;

            playerInventory.Consume(inventorySlot);
        }
    }
}
