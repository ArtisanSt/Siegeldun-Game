using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject playerEntity;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Image img;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private GameObject consumableSlot;

    void Awake()
    {
        playerEntity = GameObject.Find("Player");
        playerInventory = playerEntity.GetComponent<Inventory>();
    }

    void Update()
    {
        if (!PauseMechanics.isPlaying) return; 

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
