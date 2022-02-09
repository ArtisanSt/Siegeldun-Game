using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject playerEntity;
    private Inventory playerInventory;
    private Image img;
    private GameObject weaponSlot;
    private GameObject consumableSlot;


    void Awake()
    {
        weaponSlot = GameObject.Find("/GUI/Inventory/EqpSlotWeapon");
        consumableSlot = GameObject.Find("/GUI/Inventory/EqpConsumableBg/EqpSlotConsumable");
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (playerEntity == null)
        {
            playerEntity = GameObject.Find("Player");
            if (playerEntity != null) { playerInventory = playerEntity.GetComponent<Inventory>(); }
        }

        if((int)(gameObject.name[7] - '0') == playerInventory.selectedSlot)
        {
            img.color = Color.green;
        }
        else
        {
            img.color = Color.white;
        }

        if (gameObject == playerInventory.eqpSlotsCol["Weapon"].curSlot || gameObject == playerInventory.eqpSlotsCol["Consumable"].curSlot)
        {
            Image childImage = transform.GetChild(0).GetComponent<Image>();
            childImage.color = new Color(childImage.color.r, childImage.color.g, childImage.color.b, .5f);
        }
        else if (transform.childCount > 0)
        {
            Image childImage = transform.GetChild(0).GetComponent<Image>();
            childImage.color = new Color(childImage.color.r, childImage.color.g, childImage.color.b, 1f);
        }
    }

    public void OnPointerUp(PointerEventData evt)
    {
        GameObject slotGameObject = evt.pointerPress;
        string inInvOrEqp = slotGameObject.name.Substring(0, 3);
        if (inInvOrEqp == "Inv")
        {
            playerInventory.ProcessInventorySelection((int)(slotGameObject.name[7] - '0'), true);
        }
        else if (inInvOrEqp == "Eqp" && gameObject == consumableSlot)
        {
            playerInventory.Consume();
        }
    }

    // Don't Remove, still needed for pointerPress to function
    public void OnPointerDown(PointerEventData evt)
    {
    }
}
