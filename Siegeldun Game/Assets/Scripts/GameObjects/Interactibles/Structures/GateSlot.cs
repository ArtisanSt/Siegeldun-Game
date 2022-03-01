using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GateSlot : Structures, IGateSlot
{
    [Header("GATE SLOT SETTINGS", order = 0)]
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] Inventory playerInventory;
    private bool _slotted = false;
    public bool slotted { get { return _slotted; } protected set { _slotted = value; } }

    protected override void Awake()
    {
        base.Awake();
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    protected override void Update()
    {
        base.Update();
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected && playerInventory == null) return;

        int inventorySlot = playerInventory.FindItem(crystalPrefab);
        if (inventorySlot != -1)
        {
            //Transform crystal = Instantiate(1, new Vector2(0, 0.1f), crystalPrefab, transform).transform;
            Transform crystal = Instantiate(crystalPrefab, new Vector3(transform.position.x, transform.position.y + 0.1f, 0), Quaternion.identity, transform).transform;
            crystal.parent = transform;
            crystal.GetComponent<Item>().ChangeAmount(1);
            crystal.GetComponent<IInteractible>().DisableInteract();
            ToggleInteractible();
            slotted = true;

            playerInventory.Consume(inventorySlot);
        }
    }
}
