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

            Debug.Log(inInvOrEqp);
            if (inInvOrEqp == "Inv")
            {
                playerEntity.GetComponent<Player>().itemChosen = (int)slotGameObject.name[7];
                playerEntity.GetComponent<Player>().Use();
            }
            else if (inInvOrEqp == "Eqp")
            {
                playerEntity.GetComponent<Player>().ClearItem(itemSelected, -1);
            }
        }
    }

    // Don't Remove, still needed for pointerPress to function
    public void OnPointerDown(PointerEventData evt)
    {
    }
}
