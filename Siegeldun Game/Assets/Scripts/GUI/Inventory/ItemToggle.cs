using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject playerEntity;
    private Image img;
    private GameObject weaponSlot;
    private GameObject consumableSlot;


    void Awake()
    {
        playerEntity = GameObject.Find("Player");
        weaponSlot = GameObject.Find("/GUI/PlayerSlots/EqpSlotWeapon");
        consumableSlot = GameObject.Find("/GUI/PlayerSlots/EqpSlotConsumable");
        img = GetComponent<Image>();
    }

    void Update()
    {

        if (playerEntity.GetComponent<Player>().isAlive)
        {
            string inInvOrEqp = gameObject.name.Substring(0, 3);

            if (playerEntity.GetComponent<Player>().itemSelected == (int)(gameObject.name[7] - '0') && gameObject.transform.childCount > 0)
            {
                img.color = Color.magenta;
            }
            else
            {
                if (playerEntity.GetComponent<Player>().itemEquipped[0] == (int)(gameObject.name[7] - '0'))
                {
                    img.color = Color.yellow;
                }
                else if (playerEntity.GetComponent<Player>().itemEquipped[1] == (int)(gameObject.name[7] - '0'))
                {
                    img.color = Color.green;
                }
                else
                {
                    img.color = Color.white;
                }

            }
        }
        //playerEntity.GetComponent<Player>().itemSelected;
    }

    public void OnPointerUp(PointerEventData evt)
    {
        GameObject slotGameObject = evt.pointerPress;
        string inInvOrEqp = slotGameObject.name.Substring(0, 3);
        if (inInvOrEqp == "Inv")
        {
            int slotSelected = (int)(slotGameObject.name[7] - '0');
            playerEntity.GetComponent<Player>().Equip(slotSelected);
        }
        else if (inInvOrEqp == "Eqp")
        {
            playerEntity.GetComponent<Player>().Unequip(slotGameObject);
        }
    }

    // Don't Remove, still needed for pointerPress to function
    public void OnPointerDown(PointerEventData evt)
    {
    }
}
