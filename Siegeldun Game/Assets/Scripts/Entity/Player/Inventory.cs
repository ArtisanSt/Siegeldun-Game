using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Beings
{
    // Feature: Cannot bring same items unless stackable

    // ========================================= INVENTORY PROPERTIES =========================================
    [Header("INVENTORY PROPERTIES", order = 1)]
    public GameObject[] inventorySlots = new GameObject[6];
    public int[] itemProperties = new int[3]; // currentAmount, maxAmount, slotNumber
    public Dictionary<string, int[]> inventoryItems = new Dictionary<string, int[]>();
    public Item itemInteracted;
    private GameObject playerEntity;

    public GameObject weaponSlot;
    public GameObject consumableSlot;

    private bool pickUp;
    private bool isOn;


    // ========================================= ITEM EFFECT =========================================
    public Dictionary<string, Dictionary<string, float>> effectDict = new Dictionary<string, Dictionary<string, float>>()
    {
        ["HP"] = new Dictionary<string, float>(),
        ["Stamina"] = new Dictionary<string, float>(),
        ["Damage"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["CritHit"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["CritChance"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["MVSpeed"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, distance
        ["JumpHeight"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["AttackSpeed"] = new Dictionary<string, float>() //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
    };

    // Start is called before the first frame update
    protected void InventoryInitialization()
    {
        playerEntity = rBody.gameObject;

        for(int i=0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = GameObject.Find("InvSlot" + (char)(65+i));
        };

        weaponSlot = GameObject.Find("EqpSlotWeapon");
        consumableSlot = GameObject.Find("EqpSlotConsumable");
    }

    // Update is called once per frame
    protected void InventoryUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            AddItem(col.gameObject);
        }
    }


    protected void AddItem(GameObject pickedItem)
    {
        if (!curProcess.Contains(pickedItem.GetInstanceID()))
        {
            curProcess.Add(pickedItem.GetInstanceID());

            isOn = GameObject.Find("UI_System").GetComponent<InventoryEvents>().invOn;
            itemInteracted = pickedItem.GetComponent<Item>();
            bool isPickedUp = false;

            // Has same Item in inventory
            if (inventoryItems.ContainsKey(itemInteracted.itemName))
            {
                itemProperties = inventoryItems[itemInteracted.itemName];
                if (itemProperties[0] < itemProperties[1])
                {
                    itemProperties[0]++;
                    inventoryItems[itemInteracted.itemName] = itemProperties;
                    isPickedUp = true;
                }
            }
            else
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    GameObject curSlot = inventorySlots[i];
                    if (curSlot.transform.childCount == 0)
                    {
                        Debug.Log(itemInteracted.itemType + itemInteracted.itemName + " picked up!");
                        itemProperties = new int[3] { 1, itemInteracted.maxQuantity, i };
                        inventoryItems.Add(itemInteracted.itemName, itemProperties);

                        GameObject icon = (GameObject)Instantiate(itemInteracted.itemPrefab, curSlot.transform, false);
                        icon.name = "Inv_" + itemInteracted.itemName;
                        if (isOn)
                            icon.GetComponent<Image>().enabled = true;
                        else if (!isOn)
                            icon.GetComponent<Image>().enabled = false;
                        isPickedUp = true;
                        break;
                    }
                }
            }

            if (isPickedUp)
            {
                Destroy(pickedItem);
                StartCoroutine(ClearID(pickedItem.GetInstanceID(), 1));
            }
        }
    }


    protected void RemoveItem(GameObject pickedItem, int amount = 1)
    {
        itemInteracted = pickedItem.GetComponent<Item>();
        itemProperties = inventoryItems[itemInteracted.itemName];

        if (itemInteracted.itemType == "Consumable")
        {
            itemProperties[0] -= amount;
            if (itemProperties[0] == 0)
            {
                ClearItem(pickedItem);
                Destroy(GameObject.Find("Inv_" + itemInteracted.itemName));
            }

            Debug.Log(itemInteracted.itemName + " used");
        }
    }


    public void ClearItem(GameObject pickedItem)
    {
        inventoryItems.Remove(itemInteracted.itemName);
        Destroy(pickedItem);
    }


    protected void Consume()
    {
        if (consumableSlot.transform.childCount > 0)
        {
            bool didEffect = false;
            GameObject equippedConsumable = consumableSlot.transform.GetChild(0).gameObject;
            this.effectDict = equippedConsumable.GetComponent<Consumable>().effectDict;
            foreach (KeyValuePair<string, Dictionary<string, float>> eachEffect in this.effectDict)
            {
                if (eachEffect.Value["hasEffect"] == 1f)
                {
                    switch (eachEffect.Key)
                    {
                        case "HP":
                            didEffect = HPRegen(eachEffect.Value["effectParam"], (eachEffect.Value["effectSpeed"] == 0f) ? "instant" : "overtime", eachEffect.Value["effectTimer"]);
                            break;
                        case "Stamina":
                            didEffect = StamRegen(eachEffect.Value["effectParam"], (eachEffect.Value["effectSpeed"] == 0f) ? "instant" : "overtime", eachEffect.Value["effectTimer"]);
                            break;
                        case "Damage":
                            break;
                        case "CritHit":
                            break;
                        case "CritChance":
                            break;
                        case "MVSpeed":
                            break;
                        case "JumpHeight":
                            break;
                        case "AttackSpeed":
                            break;
                    }
                }
            }

            if (didEffect)
            {
                RemoveItem(equippedConsumable, 1);
            }
        }
    }
}
