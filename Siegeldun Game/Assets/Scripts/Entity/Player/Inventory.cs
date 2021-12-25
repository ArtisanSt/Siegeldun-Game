using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemProperties
{
    public bool isFull = false;
    public GameObject inventoryBg = null;
    public GameObject inventorySlot = null;
    public string itemName = null;
    public int currentAmount = 0;
    public int maxAmount = 0;

    public ItemProperties()
    {
        this.isFull = false;
        this.itemName = null;
        this.currentAmount = 0;
        this.maxAmount = 0;
    }

    public ItemProperties(GameObject inventoryBg, GameObject inventorySlot)
    {
        this.inventoryBg = inventoryBg;
        this.inventorySlot = inventorySlot;
    }

    public bool[] setValues(string itemName, int currentAmount, int maxAmount)
    {
        this.isFull = false;
        this.itemName = itemName;
        this.currentAmount = currentAmount;
        this.maxAmount = maxAmount;

        return new bool[2] { true, false };
    }

    public bool[] changeValues(int changeAmount)
    {
        bool processSuccessful = false;
        bool removeInInv = false;
        if (currentAmount + changeAmount >= 0 || currentAmount + changeAmount <= maxAmount)
        {
            currentAmount += changeAmount;
            processSuccessful = true;

            if (currentAmount == 0)
            {
                removeInInv = true;
            }
            else if (currentAmount == maxAmount)
            {
                isFull = true;
            }
        }

        return new bool[2] { processSuccessful, removeInInv };
    }
}

public class Inventory : Beings
{
    // Feature: Cannot bring same items unless stackable

    // ========================================= INVENTORY PROPERTIES =========================================
    [Header("INVENTORY PROPERTIES", order = 1)]
    public GameObject inventoryBg;
    protected GameObject itemColliding;
    protected List<ItemProperties> inventorySlots = new List<ItemProperties>();
    protected GameObject playerEntity;
    public int itemChosen;

    public GameObject weaponSlot;

    // Start is called before the first frame update
    protected void InventoryInitialization()
    {
        playerEntity = rBody.gameObject;
        inventoryBg = GameObject.Find("/GUI/PlayerSlots/InventoryBackground");

        for (int i=0; i < inventoryBg.transform.childCount; i++)
        {
            GameObject invSlot = GameObject.Find("/GUI/PlayerSlots/InventoryBackground/InvSlot" + (char)(65 + i));
            inventorySlots.Add(new ItemProperties(inventoryBg, invSlot));
        };

        weaponSlot = GameObject.Find("EqpSlotWeapon");

        itemChosen = 0;
    }

    // Update is called once per frame
    protected void InventoryUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            itemColliding = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        itemColliding = null;
    }

    protected void AddItem(GameObject pickedItem)
    {
        // ID is to cancel repetitive processing of same GameObject
        if (pickedItem != null && !curProcess.Contains(pickedItem.GetInstanceID()))
        {
            curProcess.Add(pickedItem.GetInstanceID());

            Item itemInteracted = pickedItem.GetComponent<Item>();

            bool[] isSuccess = new bool[2] { false, false }; // processSuccessful, removeInInv
            bool isDone = false;
            int invFullCount = 0;
            // Checks if item is in inventory
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                ItemProperties itemProp = inventorySlots[i];
                invFullCount = (itemProp.isFull) ? invFullCount++ : invFullCount ;
                if (itemProp.itemName == null || (itemProp.itemName == itemInteracted.itemName && itemProp.isFull))
                {
                    Debug.Log($"{itemProp.itemName} : {itemProp.isFull} {itemProp.currentAmount} {itemProp.maxAmount}");
                    continue;
                }
                else if (itemProp.itemName == itemInteracted.itemName && !itemProp.isFull)
                {
                    isSuccess = itemProp.changeValues(1);
                    isDone = isSuccess[0];
                    break;
                }
            }

            // Item not in inventory
            if (!isDone && invFullCount < 6)
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    ItemProperties itemProp = inventorySlots[i];
                    // Slot has no item in it
                    if (itemProp.itemName == null)
                    {
                        isSuccess = itemProp.setValues(itemInteracted.itemName, 1, itemInteracted.maxQuantity);
                        Debug.Log($"{itemProp.inventorySlot.transform}");
                        Debug.Log($"{itemProp.inventorySlot.transform}");
                        GameObject icon = (GameObject)Instantiate(itemInteracted.itemPrefab, itemProp.inventorySlot.transform, false);
                        icon.name = "Inv_" + itemInteracted.itemName;
                        break;
                    }
                    else if (itemProp.itemName == itemInteracted.itemName)
                    {
                        if (!itemProp.isFull)
                        {
                            isSuccess = itemProp.changeValues(1);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            if (isSuccess[0])
            {
                Destroy(pickedItem);
                StartCoroutine(ClearID(pickedItem.GetInstanceID(), 1));
                Debug.Log(itemInteracted.itemName + " picked up!");
            }
        }
    }


    protected void RemoveItem(GameObject pickedItem, int slotNumber, int amount = 1)
    {
        Item itemInteracted = pickedItem.GetComponent<Item>();

        bool[] isSuccess = new bool[2] { false, false }; // processSuccessful, removeInInv

        ItemProperties itemProp = inventorySlots[slotNumber];
        // Slot has no item in it
        if (itemProp.itemName != null)
        {
            isSuccess = itemProp.changeValues(-1);
        }

        if (isSuccess[1])
        {
            ClearItem(pickedItem, slotNumber);
            Debug.Log(itemInteracted.itemName + " used!");
        }
    }

    public void ClearItem()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            ItemProperties itemProp = inventorySlots[i];
            if (itemProp.inventorySlot.transform.childCount > 0)
            {
                Destroy(itemProp.inventorySlot.transform.GetChild(0).gameObject);
            }
            inventorySlots[i] = new ItemProperties();
        }
    }

    public void ClearItem(GameObject pickedItem, int slotNumber)
    {
        if (slotNumber >= 0)
        {
            inventorySlots[slotNumber] = new ItemProperties();
        }
        Destroy(pickedItem);
    }

    protected void ConsumeControls()
    {
        // Pickup items or Interact
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddItem(itemColliding);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemChosen = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemChosen = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            itemChosen = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            itemChosen = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            itemChosen = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            itemChosen = 5;
        }

        // Change the box UI in the UI but might be moved in other gameObject
        if (itemChosen == 0)
        {

        }
    }

    public void Use()
    {
        ItemProperties itemProp = inventorySlots[itemChosen];
        if (itemProp.itemName != null)
        {
            GameObject pickedItem = itemProp.inventorySlot.transform.GetChild(0).gameObject;
            if (pickedItem.GetComponent<Item>().itemType == "Consumable")
            {
                Consume(itemProp, itemChosen);
            }
            else if (pickedItem.GetComponent<Item>().itemType == "Weapon")
            {
                Equip(pickedItem);
            }
        }
    }

    protected void Equip(GameObject pickedItem)
    {
        GameObject newEqpItem = (GameObject)Instantiate(pickedItem, weaponSlot.transform, false);
        newEqpItem.name = "Eqp_" + pickedItem.GetComponent<Item>().itemName;
    }

    protected void Consume(ItemProperties itemProp, int itemChosen)
    {
        GameObject pickedItem = itemProp.inventorySlot.transform.GetChild(0).gameObject;
        Dictionary<string, Dictionary<string, float>> effectDict = pickedItem.GetComponent<Consumable>().effectDict;
        bool didEffect = false;
        foreach (KeyValuePair<string, Dictionary<string, float>> eachEffect in effectDict)
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
            RemoveItem(pickedItem, itemChosen, 1);
        }
    }
}
