using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IDropHandler
{
    private InventorySystem inventory;

    public void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<InventorySystem>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log(eventData.pointerDrag.name + " on " + transform.name);
            eventData.pointerDrag.GetComponent<DragDrop>().droppedOnSlot = true;
            eventData.pointerDrag.transform.position = this.transform.position;

            if(transform.name == "Main_SlotB")
                if(eventData.pointerDrag.GetComponent<Item>().itemType == "Consumable")
                    inventory.consumeSlot = eventData.pointerDrag.GetComponent<Item>().itemName;
                else
                    eventData.pointerDrag.GetComponent<DragDrop>().droppedOnSlot = false;
            else if(transform.name == "Main_SlotA")
                if(eventData.pointerDrag.GetComponent<Item>().itemType == "Weapon")
                    inventory.weaponSlot = eventData.pointerDrag.GetComponent<Item>().itemName;
                else
                    eventData.pointerDrag.GetComponent<DragDrop>().droppedOnSlot = false;
            //Debug.Log(eventData.pointerDrag.name);
        }
    }
}