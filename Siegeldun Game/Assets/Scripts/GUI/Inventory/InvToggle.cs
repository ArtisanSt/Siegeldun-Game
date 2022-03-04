using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvToggle : MonoBehaviour
{
    public bool isOn = false;
    public GameObject inventoryBg;

    // Start is called before the first frame update
    void Start()
    {
        inventoryBg = GameObject.Find("/Game System/Inventory/InventoryBackground");
        InvBGUpdate();
    }


    protected void InvBGUpdate()
    {
        gameObject.transform.localScale = new Vector3(.5f, (isOn) ? -.25f : .25f, .5f);
        inventoryBg.GetComponent<Image>().enabled = isOn;
        foreach (Transform category in inventoryBg.transform)
        {
            switch (category.name)
            {
                case "InvSlots":
                    foreach (Transform slot in category.transform)
                    {
                        slot.gameObject.GetComponent<Image>().enabled = isOn;
                        if (slot.childCount > 0)
                        {
                            foreach (Transform item in slot.transform)
                            {
                                item.gameObject.GetComponent<Image>().enabled = isOn;
                            }
                        }
                    }
                    break;

                case "InventoryAmountTexts":
                    foreach (Transform text in category.transform)
                    {
                        text.gameObject.GetComponent<Text>().enabled = isOn;
                    }
                    break;

                case "InventoryKeyBind":
                    foreach (Transform text in category.transform)
                    {
                        text.gameObject.GetComponent<Text>().enabled = isOn;
                    }
                    break;
            }
        }
    }

    public void ToggleButton()
    {
        isOn = !isOn;
        InvBGUpdate();
    }
}
