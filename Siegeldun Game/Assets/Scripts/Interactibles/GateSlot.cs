using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GateSlot : Interactibles
{
    [SerializeField] Transform crystalPrefab;
    Inventory playerInventory;
    public bool slotted = false;

    void Awake()
    {
        isItem = false;
        isIcon = false;
        canInteract = true;
        isSelected = false;

        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        Interaction();
    }

    // Interaction Event
    void Interaction()
    {
        if(isSelected)
        {
            if(Input.GetKeyDown(KeyCode.F) && !slotted)
            {
                int inventorySlot = playerInventory.CheckItem("Crystal");
                if(inventorySlot != -1)
                {
                    Transform crystal = Instantiate(crystalPrefab, new Vector3(this.transform.position.x, this.transform.position.y + 0.1f, 0), Quaternion.identity);
                    crystal.transform.parent = this.transform;
                    GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
                    canInteract = false;
                    slotted = true;

                    playerInventory.RemoveItem(inventorySlot);
                }
            }
        }
    }
}
