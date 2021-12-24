using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Inventory inventorySystem;

    public void Awake()
    {
        inventorySystem = GameObject.Find("Player").GetComponent<Inventory>();
    }

    public void OnPointerUp(PointerEventData evt)
    {
        GameObject slotGameObject = evt.pointerPress;
        if (slotGameObject.transform.childCount > 0)
        {
            GameObject itemSelected = slotGameObject.transform.GetChild(0).gameObject;
            GameObject playerEntity = GameObject.Find("Player");
            string inInvOrEqp = itemSelected.name.Substring(0,3);
            Debug.Log(itemSelected);

            Debug.Log(inInvOrEqp);
            if (inInvOrEqp == "Inv")
            {
                GameObject EqpSlot;
                if (itemSelected.GetComponent<Item>().itemType == "Weapon")
                {
                    EqpSlot = playerEntity.GetComponent<Player>().weaponSlot;
                }
                else
                {
                    EqpSlot = playerEntity.GetComponent<Player>().consumableSlot;
                }

                if (EqpSlot.transform.childCount > 0)
                {
                    GameObject itemUnequip = EqpSlot.transform.GetChild(0).gameObject;
                    Destroy(itemUnequip);
                }

                GameObject newEqpItem = (GameObject)Instantiate(itemSelected, EqpSlot.transform, false);
                newEqpItem.name = "Eqp_" + itemSelected.GetComponent<Item>().itemName;
            }
            else if (inInvOrEqp == "Eqp")
            {
                playerEntity.GetComponent<Player>().ClearItem(itemSelected);
            }
        }
    }

    // Don't Remove, still needed for pointerPress to function
    public void OnPointerDown(PointerEventData evt)
    {
    }
}
