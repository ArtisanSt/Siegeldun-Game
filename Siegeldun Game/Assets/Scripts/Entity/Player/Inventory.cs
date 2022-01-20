using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemProperties
{
    public bool isFull = false;
    public GameObject inventoryBg = null;
    public GameObject inventorySlot = null;
    public GameObject itemObject = null;
    public string itemName = null;
    public int currentAmount = 0;
    public int maxAmount = 0;

    public ItemProperties(GameObject inventoryBg, GameObject inventorySlot)
    {
        this.inventoryBg = inventoryBg;
        this.inventorySlot = inventorySlot;
    }

    public bool[] setValues(GameObject itemObject, string itemName, int currentAmount, int maxAmount)
    {
        this.itemObject = itemObject;
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
        if (currentAmount + changeAmount >= 0 && currentAmount + changeAmount <= maxAmount)
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
            else
            {
                isFull = false;
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
    public int itemSelected;
    public int[] itemEquipped = new int[2]; // Weapon, Consumable 

    public GameObject weaponSlot;
    public GameObject consumableSlot;

    private Interactor interactorComponent;

    [SerializeField] List<Text> itemAmountTexts;
    [SerializeField] Text consumeSlotAmountText;


    // ========================================= INITIALIZATION METHODS =========================================
    // Start is called before the first frame update
    protected void InventoryInit()
    {
        inventoryBg = GameObject.Find("/GUI/PlayerSlots/InventoryBackground");

        for (int i=0; i < 6; i++)
        {
            GameObject invSlot = GameObject.Find($"/GUI/PlayerSlots/InventoryBackground/InvSlots/InvSlot{i}");
            itemAmountTexts.Add(GameObject.Find($"/GUI/PlayerSlots/InventoryBackground/InventoryAmountTexts/InvSlot{i}Amount").GetComponent<Text>());
            UpdateText(itemAmountTexts[i], 0, i);
            itemAmountTexts[i].text = "";
            inventorySlots.Add(new ItemProperties(inventoryBg, invSlot));
        };

        weaponSlot = GameObject.Find("/GUI/PlayerSlots/EqpSlotWeapon");
        consumableSlot = GameObject.Find("/GUI/PlayerSlots/EqpSlotConsumable");
        consumeSlotAmountText = GameObject.Find("/GUI/PlayerSlots/ConsumeSlotAmount").GetComponent<Text>();
        UpdateText(consumeSlotAmountText, 0, 0);

        itemSelected = -1;
        itemEquipped = new int[2] { -1, -1 };

        interactorComponent = GetComponent<Interactor>();
    }

    // ========================================= MAIN METHODS =========================================
    // Update is called once per frame
    protected void InventoryUpdate()
    {

    }

    // ========================================= CONTROL METHODS =========================================
    protected void ConsumeControls()
    {
        // Pickup items or Interact
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddItem(interactorComponent.curSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Equip(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Equip(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Equip(5);
        }

        // Use Item in Inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            Consume(consumableSlot, true);
        }

        // Use Item in Inventory
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw();
        }

        // Pseudo Damage Taken 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject.Find("InvToggle").GetComponent<InvToggle>().ToggleButton();
        }
    }

    // ========================================= INVENTORY METHODS =========================================
    protected void AddItem(GameObject pickedItem)
    {
        // ID is to cancel repetitive processing of same GameObject
        if (pickedItem != null && pickedItem.GetComponent<Interactibles>().isItem && !curProcess.Contains(pickedItem.GetInstanceID()))
        {
            curProcess.Add(pickedItem.GetInstanceID());

            Item itemInteracted = pickedItem.GetComponent<Item>();
            bool[] isSuccess = new bool[2] { false, false }; // processSuccessful, removeInInv
            int invFullCount = 0;
            int slotSaved = 0;

            // Checks if item is in inventory
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                ItemProperties itemProp = inventorySlots[i];
                invFullCount = (itemProp.isFull) ? invFullCount++ : invFullCount;

                if (itemProp.itemName == itemInteracted.itemName && !itemProp.isFull)
                {
                    isSuccess = itemProp.changeValues(1);
                    UpdateText(itemAmountTexts[i], itemProp.currentAmount, i);
                    slotSaved = i;
                    break;
                }
            }

            // Item not in inventory
            if (!isSuccess[0] && invFullCount < 6)
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    ItemProperties itemProp = inventorySlots[i];
                    // Slot has no item in it
                    if (itemProp.itemName == null)
                    {
                        GameObject newAddItem = (GameObject)Instantiate(itemInteracted.iconPrefab, itemProp.inventorySlot.transform, false);
                        isSuccess = itemProp.setValues(newAddItem, itemInteracted.itemName, itemInteracted.curQuantity, itemInteracted.maxQuantity);
                        newAddItem.name = "Inv_" + itemInteracted.itemName;
                        newAddItem.GetComponent<Image>().enabled = GameObject.Find("InvToggle").GetComponent<InvToggle>().isOn;
                        UpdateText(itemAmountTexts[i], itemProp.currentAmount, i);
                        slotSaved = i;
                        break;
                    }
                }
            }

            if (isSuccess[0])
            {
                Destroy(pickedItem);
                StartCoroutine(ClearID(pickedItem.GetInstanceID(), 1));
                Debug.Log(itemInteracted.itemName + " picked up!");
                Debug.Log(weaponSlot.transform.childCount < 1 || consumableSlot.transform.childCount < 1);
                if (weaponSlot.transform.childCount < 1 || consumableSlot.transform.childCount < 1)
                {
                    Equip(slotSaved);
                }
                if (slotSaved == itemEquipped[1])
                {
                    UpdateText(consumeSlotAmountText, inventorySlots[itemEquipped[1]].currentAmount, itemEquipped[1]);
                }
            }
        }
    }

    protected void RemoveItem(int slotNumber, int amount = 1)
    {
        ItemProperties itemProp = inventorySlots[slotNumber];
        bool[] isSuccess = new bool[2] { false, false }; // processSuccessful, removeInInv

        // Slot has no item in it
        if (itemProp.itemName != null)
        {
            isSuccess = itemProp.changeValues(-1);
        }

        if (isSuccess[1])
        {
            ClearItem(slotNumber);
        }

        if (isSuccess[0])
        {
            UpdateText(itemAmountTexts[slotNumber], itemProp.currentAmount, slotNumber);
        }
    }

    protected void UpdateText(Text textGameObject , int newAmount, int slotNumber)
    {
        textGameObject.text = (newAmount != 0) ? $"{inventorySlots[slotNumber].currentAmount}" : "";
    }

    // Delete all
    public void ClearItem()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            ItemProperties itemProp = inventorySlots[i];
            if (itemProp.itemObject != null)
            {
                ClearItem(i);
            }
            UpdateText(itemAmountTexts[i], itemProp.currentAmount, i);
        }
    }

    // Delete one item
    public void ClearItem(int slotNumber)
    {
        GameObject pickedItem = inventorySlots[slotNumber].inventorySlot.transform.GetChild(0).gameObject;
        GameObject eqpSlot = (slotNumber == itemEquipped[0]) ? weaponSlot : ((slotNumber == itemEquipped[1]) ? consumableSlot : null);
        if (eqpSlot != null)
        {
            Unequip(eqpSlot);
        }
        Destroy(pickedItem) ;
        inventorySlots[slotNumber] = new ItemProperties(inventoryBg, inventorySlots[slotNumber].inventorySlot);
    }

    // Checks if a slot has item attached or not
    public bool SelectSlot(int slotSelected)
    {
        ItemProperties itemProp = inventorySlots[slotSelected];
        if (itemProp.itemName != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ========================================= USE METHODS =========================================
    public void Equip(int itemSelected)
    {
        if (SelectSlot(itemSelected))
        {
            this.itemSelected = itemSelected;
            ItemProperties itemProp = inventorySlots[itemSelected];
            GameObject pickedItem = itemProp.inventorySlot.transform.GetChild(0).gameObject;
            Debug.Log(pickedItem.GetComponent<Item>().itemType);
            if (pickedItem.GetComponent<Item>().itemType == "Consumable")
            {
                if (consumableSlot.transform.childCount > 0)
                {
                    Destroy(consumableSlot.transform.GetChild(0).gameObject);
                }
                GameObject newEqpItem = (GameObject)Instantiate(pickedItem, consumableSlot.transform, false);
                newEqpItem.name = "Eqp_" + pickedItem.GetComponent<Item>().itemName;
                newEqpItem.GetComponent<Image>().enabled = true;
                itemEquipped[1] = itemSelected;
                UpdateText(consumeSlotAmountText, inventorySlots[itemEquipped[1]].currentAmount, itemEquipped[1]);
            }
            else if (pickedItem.GetComponent<Item>().itemType == "Weapon")
            {
                if (weaponSlot.transform.childCount > 0)
                {
                    Destroy(weaponSlot.transform.GetChild(0).gameObject);
                }
                GameObject newEqpItem = (GameObject)Instantiate(pickedItem, weaponSlot.transform, false);
                newEqpItem.name = "Eqp_" + pickedItem.GetComponent<Item>().itemName;
                newEqpItem.GetComponent<Image>().enabled = true;
                itemEquipped[0] = itemSelected;

                StatsUpdate();
            }
        }
    }

    public void Unequip(GameObject eqpSlot)
    {
        if (eqpSlot.transform.childCount > 0)
        {
            Destroy(eqpSlot.transform.GetChild(0).gameObject);
            itemEquipped[(eqpSlot == weaponSlot) ? 0 : 1] = -1 ;
        }
        UpdateText(consumeSlotAmountText, 0, itemEquipped[1]);
    }

    public void Consume(GameObject pickedItem, bool isSlot)
    {
        if (isSlot)
        {
            if (pickedItem.transform.childCount > 0)
            {
                pickedItem = pickedItem.transform.GetChild(0).gameObject;
            }
            else
            {
                return;
            }
        }

        if (pickedItem != null)
        {
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
                RemoveItem(itemEquipped[1], 1);
                UpdateText(consumeSlotAmountText, inventorySlots[itemEquipped[1]].currentAmount, itemEquipped[1]);
            }
        }
    }

    protected void Throw()
    {
        if (isGrounded && inventorySlots[itemSelected].itemObject != null)
        {
            Debug.Log(inventorySlots[itemSelected].itemObject);
            GameObject newDrop = Drop(1, 0, 0, inventorySlots[itemSelected].itemObject.GetComponent<Item>().itemPrefab);
            newDrop.GetComponent<Item>().curQuantity = inventorySlots[itemSelected].currentAmount;
            ClearItem(itemSelected);
        }
    }

    protected void StatsUpdate()
    {
        weaponGameObject = (weaponSlot.transform.childCount > 0) ? weaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>() : null;

        if (weaponGameObject == null)
        {
            entityDamage = 5f;
            attackSpeed = 2;
            attackDelay = 0.3f;
            critHit = .1f;
            critChance = 1000;
            attackRange = 0.3f;
            EqWeaponStamCost = 0f;
        }
        else
        {
            entityDamage = weaponGameObject.damage;
            attackSpeed = weaponGameObject.attackSpeed;
            attackDelay = weaponGameObject.attackDelay;
            critHit = weaponGameObject.critHit;
            critChance = weaponGameObject.critChance;
            attackRange = weaponGameObject.attackRange;
            EqWeaponStamCost = weaponGameObject.staminaCost;
        }
    }
}
