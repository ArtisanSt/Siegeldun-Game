using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEvents : MonoBehaviour
{
    GameObject mainInventory; // = GameObject.Find("Inventory Background");
    public bool invOn;

    private void Start()
    {
        mainInventory = GameObject.Find("InventoryBackground");
        invOn = true;
    }

    protected void OffMainInventory()
    {
        ToggleInventory(false);
    }

    protected void OnMainInventory()
    {
        ToggleInventory(true);
    }

    private void ToggleInventory(bool setting)
    {
        mainInventory.SetActive(setting);
        invOn = setting;
    }
}
