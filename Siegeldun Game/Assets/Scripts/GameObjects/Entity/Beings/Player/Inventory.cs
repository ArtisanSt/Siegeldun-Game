using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlotProperties
{
    public GameObject eqpSlot { get; private set; }

    public GameObject curSlot = null;
    public GameObject curItem = null;

    public EquippedSlotProperties(GameObject eqpSlot)
    {
        this.eqpSlot = eqpSlot;
    }
}

public class Inventory: BaseObject
{
    // ========================================= INVENTORY PROPERTIES =========================================
    [SerializeField] public GameObject inventoryBg;
    protected GameObject itemColliding;
    protected List<GameObject[]> inventorySlots = new List<GameObject[]>(); // idx: {slot, item}

    private int _selectedSlot = 0;
    public int selectedSlot { get { return _selectedSlot; } protected set { _selectedSlot = value; } }

    public Dictionary<string, EquippedSlotProperties> eqpSlotsCol { get; protected set; }

    [SerializeField] private Interactor interactorComponent;

    [SerializeField] List<Text> itemAmountTexts;
    [SerializeField] Text consumeSlotAmountText;


    // ========================================= INITIALIZATION METHODS =========================================
    void Awake()
    {
        GameMechanicsPropInit();

        inventoryBg = GameObject.Find("/GUI/Inventory/InventoryBackground");

        for (int i = 0; i < 6; i++)
        {
            GameObject invSlot = GameObject.Find($"/GUI/Inventory/InventoryBackground/InvSlots/InvSlot{i}");
            inventorySlots.Add(new GameObject[2] { invSlot, null });
            itemAmountTexts.Add(GameObject.Find($"/GUI/Inventory/InventoryBackground/InventoryAmountTexts/InvSlot{i}Amount").GetComponent<Text>());
            UpdateText(itemAmountTexts[i], 0);
            itemAmountTexts[i].text = "";
        };

        eqpSlotsCol = new Dictionary<string, EquippedSlotProperties>()
        {
            ["Weapon"] = new EquippedSlotProperties(GameObject.Find("/GUI/Inventory/EqpSlotWeapon")),
            ["Consumable"] = new EquippedSlotProperties(GameObject.Find("/GUI/Inventory/EqpConsumableBg/EqpSlotConsumable")),
        };

        consumeSlotAmountText = GameObject.Find("/GUI/Inventory/EqpConsumableBg/ConsumeSlotAmount").GetComponent<Text>();

        interactorComponent = GetComponent<Interactor>();
    }

    // ========================================= MAIN METHODS =========================================
    // Update is called once per frame
    void Update()
    {
        if (!PauseMechanics.isPlaying) return;
        InventoryControls();
        UpdateText();
    }

    // ========================================= CONTROL METHODS =========================================
    protected void InventoryControls()
    {
        // MOVE TO INTERACTOR COMPONENT //////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.F))
        {
            ProcessInteractorSelection(interactorComponent.curSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ProcessInventorySelection(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ProcessInventorySelection(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ProcessInventorySelection(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ProcessInventorySelection(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ProcessInventorySelection(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ProcessInventorySelection(5);
        }

        // Use Item in Inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            Consume();
        }

        // Use Item in Inventory
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw(selectedSlot);
        }

        // Pseudo Damage Taken 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject.Find("InvToggle").GetComponent<InvToggle>().ToggleButton();
        }
    }

    protected void UpdateText()
    {
        // Each Item in Inventory
        for (int i = 0; i < itemAmountTexts.Count; i++)
        {
            int curAmount = 0;

            if (inventorySlots[i][1] != null)
            {
                curAmount = inventorySlots[i][1].GetComponent<Item>().curQuantity;
            }

            UpdateText(itemAmountTexts[i], curAmount);
        }

        // Equipped Consumable Item
        UpdateText(consumeSlotAmountText, (eqpSlotsCol["Consumable"].curItem != null) ? eqpSlotsCol["Consumable"].curItem.GetComponent<Item>().curQuantity : 0 );
        // UpdateText(consumeSlotAmountText, (eqpSlotsCol["Consumable"].eqpItem != null) ? consumableSlot.transform.GetChild(0).GetComponent<Item>().curQuantity : 0 );
    }

    protected void UpdateText(Text curText, int curAmount)
    {
        curText.text = (curAmount != 0) ? $"{curAmount}" : "";
    }




    // ========================================= INTERACTOR SELECTION PROCESSOR =========================================
    // MIGHT BE MOVED TO INTERACTOR COMPONENT
    protected bool ProcessInteractorSelection(GameObject selectedObject) // Returns if process is successful or not
    {
        if (selectedObject == null) return false;

        SoundManager.Instance.PlayInteract();
        bool outcome = false;
        string objectClassification = selectedObject.GetComponent<Interactibles>().objectClassification;
        switch (objectClassification)
        {
            case "ITEM":
                outcome = AddToInventory(selectedObject);
                break;

            case "STRUCTURE":
                selectedObject.GetComponent<IInteractible>().Interact();
                break;
        }

        return outcome;
    }




    // ========================================= INVENTORY SELECTION PROCESSOR =========================================
    public void ProcessInventorySelection(int selectedSlot, bool forceEquip = false)
    {
        // If already selected, the item will be equipped
        if (this.selectedSlot == selectedSlot) forceEquip = true;
        else SelectSlot(selectedSlot);

        if (forceEquip) { bool x = Equip(selectedSlot); }

        /*
        GameObject curSlot = inventorySlots[selectedSlot][0];
        Image curSlotImage = curSlot.GetComponent<Image>();
        curSlotImage.color = Color.magenta;
        */
    }


    // Checks if a slot has item attached or not
    public void SelectSlot(int selectedSlot)
    {
        this.selectedSlot = selectedSlot;
    }



    // ========================================= FIND ITEM METHODS =========================================
    public int FindItem(string itemName) // -1 = not found, !-1 = found
    {
        int itemIdx = -1;
        for (int i=0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i][1] != null && inventorySlots[i][1].GetComponent<Item>().itemName == itemName)
            {
                itemIdx = i;
                break;
            }
        }
        return itemIdx;
    }

    public int FindItem(GameObject curItem)
    {
        return FindItem(curItem.GetComponent<Item>().itemName);
    }




    // ========================================= ADD TO INVENTORY METHODS =========================================
    protected bool AddToInventory(GameObject selectedItem)
    {
        // Forbids the Item to process twice
        bool newProcess = ProcessEvaluator((float)selectedItem.GetInstanceID(), 1);
        if (!newProcess) return false;

        Item selItemProp = selectedItem.GetComponent<Item>();

        // Finds all the same item in inventory that is not full
        List<int>[] possibleSlots = PossibleSlots(selItemProp);

        int[] isSuccess = AddItem(possibleSlots[0], possibleSlots[1], selItemProp);// {[-1: Fail, 0: Success, 1: Overflow] , [index]}

        if (isSuccess[0] >= 0)
        {
            Destroy(selectedItem);
            if (selItemProp.equippable && eqpSlotsCol[selItemProp.itemType.ToString()].curItem == null) Equip(isSuccess[1]);

            return true;
        }
        else return false;
    }

    private List<int>[] PossibleSlots(Item selItemProp) // Returns the indexes of the same not full items, and vacant slots
    {
        List<int> sameItems = new List<int>();
        List<int> vacantSlots = new List<int>();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // Conditions: not vacant, same item name, not full
            if (inventorySlots[i][1] != null && inventorySlots[i][1].GetComponent<Item>().itemName == selItemProp.itemName && !inventorySlots[i][1].GetComponent<Item>().isFull) sameItems.Add(i);

            // Condition: vacant
            else if (inventorySlots[i][1] == null) vacantSlots.Add(i);
        }

        return new List<int>[2] { sameItems, vacantSlots };
    }

    private int[] AddItem(List<int> sameItems, List<int> vacantSlots, Item selItemProp)
    {
        int isSuccess = -1; // -1: Fail, 0: Success, 1: Overflow

        int slotSaved = 0;
        // Found same item or there are vacant slots
        if (sameItems.Count > 0 || vacantSlots.Count > 0)
        {
            slotSaved = (sameItems.Count > 0) ? sameItems[0] : vacantSlots[0];
            bool isNew = (sameItems.Count > 0) ? false : true;
            isSuccess = AddItemInSlot(isNew, slotSaved, selItemProp);
        }

        return new int[2] { isSuccess, slotSaved };
    }

    private int AddItemInSlot(bool isNew, int slotNumber, Item selItemProp)
    {
        GameObject curSlot = inventorySlots[slotNumber][0];
        GameObject curItem = inventorySlots[slotNumber][1];

        int isSuccess = -1; // -1: Fail, 0: Success, 1: Overflow

        if (isNew)
        {
            curItem = (GameObject)Instantiate(selItemProp.iconPrefab, curSlot.transform, false); // New Inventory Icon Instance
            Item curItemProp = curItem.GetComponent<Item>();

            curItem.name = "Inv_" + curItemProp.itemName;
            curItem.GetComponent<Image>().enabled = GameObject.Find("InvToggle").GetComponent<InvToggle>().isOn;

            inventorySlots[slotNumber][1] = curItem;

            // Overwrites the Stats of the Icon created by the stats of the Item picked
            if (curItemProp.itemType.ToString() == "Consumable") curItem.GetComponent<Consumable>().OverwriteStats(selItemProp.GetComponent<Consumable>());
            else if (curItemProp.itemType.ToString() == "Weapon") curItem.GetComponent<Weapon>().OverwriteStats(selItemProp.GetComponent<Weapon>());
            curItemProp.OnInventory(gameObject, curSlot);

            isSuccess = 0;
        }
        else
        {
            isSuccess = curItem.GetComponent<Item>().ChangeAmount(selItemProp.curQuantity);
        }

        return isSuccess;
    }




    // ========================================= REMOVE TO INVENTORY METHODS =========================================
    public bool RemoveFromInventory(int selectedItem)
    {
        return RemoveFromInventory(inventorySlots[selectedItem][1]);
    }

    public bool RemoveFromInventory(GameObject selectedItem)
    {
        // Forbids the Item to process twice and checks if selected item exists
        bool newProcess = ProcessEvaluator((float)selectedItem.GetInstanceID(), 1);
        if (!newProcess || selectedItem == null) return false;

        // Unequips the item
        if (IsItemEquipped(selectedItem)) { bool x = Unequip(eqpSlotsCol[selectedItem.GetComponent<Item>().itemType.ToString()].eqpSlot); }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i][1] == selectedItem) inventorySlots[i][1] = null;
        }

        Destroy(selectedItem);

        return true;
    }

    protected void ClearInventory()
    {
        foreach(GameObject[] eachSlotItemPair in inventorySlots)
        {
            if (eachSlotItemPair[1] != null)
            {
                RemoveFromInventory(eachSlotItemPair[1]);
            }
        }
    }




    // ========================================= EQUIP ITEM METHODS =========================================
    protected bool IsItemEquipped(GameObject curItem)
    {
        return (IsItemEquippable(curItem)) ? eqpSlotsCol[curItem.GetComponent<Item>().itemType.ToString()].curItem == curItem : false;
    }

    protected bool IsItemEquippable(GameObject curItem)
    {
        return curItem.GetComponent<Item>().equippable;
    }

    public bool Equip(int selectedSlot)
    {
        bool isSuccess = false;

        GameObject curItem = inventorySlots[selectedSlot][1];

        if (curItem != null) isSuccess = Equip(curItem);

        return isSuccess;
    }

    public bool Equip(GameObject curItem)
    {
        if (!IsItemEquippable(curItem) || IsItemEquipped(curItem)) return false;

        Item curItemProp = curItem.GetComponent<Item>();
        GameObject eqpSlot = eqpSlotsCol[curItemProp.itemType.ToString()].eqpSlot;

        // Clears current equipped item
        Unequip(eqpSlot);

        eqpSlotsCol[curItemProp.itemType.ToString()].curSlot = curItem.transform.parent.gameObject;
        eqpSlotsCol[curItemProp.itemType.ToString()].curItem = curItem;

        GameObject newEqpItem = (GameObject)Instantiate(curItemProp.iconPrefab, eqpSlot.transform, false);
        newEqpItem.name = "Eqp_" + curItemProp.itemName;
        newEqpItem.GetComponent<Image>().enabled = true;

        newEqpItem.GetComponent<Item>().ItemReferencedTo(curItem);
        curItem.GetComponent<Item>().OnEquip(gameObject);
        if (eqpSlot == eqpSlotsCol["Weapon"].eqpSlot) gameObject.GetComponent<IWeaponizable>().SetWeapon(newEqpItem.GetComponent<Weapon>()); // Should be changed to Events //////////////////////

        return true;
    }




    // ========================================= UNEQUIP ITEM METHODS =========================================
    public bool Unequip(GameObject eqpSlot)
    {
        bool isSuccess = false;
        if (eqpSlot.transform.childCount > 0)
        {
            // Gameobject of item under the eqpslot
            GameObject eqpItem = eqpSlot.transform.GetChild(0).gameObject;
            Item eqpItemProp = eqpItem.GetComponent<Item>();

            // Gameobject of item in the invslot
            GameObject curItem = eqpSlotsCol[eqpItemProp.itemType.ToString()].curItem;
            Item curItemProp = curItem.GetComponent<Item>();

            // Unequips the current equipped item
            curItemProp.OnUnequip();
            eqpSlotsCol[eqpItemProp.itemType.ToString()].curSlot = null;
            eqpSlotsCol[eqpItemProp.itemType.ToString()].curItem = null;
            Destroy(eqpItem);

            gameObject.GetComponent<IWeaponizable>().SetWeapon(null); // Should be changed to Events //////////////////////

            isSuccess = true;
        }

        return isSuccess;
    }




    // ========================================= USE METHODS =========================================
    public bool Consume()
    {
        if (eqpSlotsCol["Consumable"].curItem == null) return false;

        GameObject curItem = eqpSlotsCol["Consumable"].curItem;
        Item curItemProp = curItem.GetComponent<Item>();

        bool isSuccess = curItemProp.OnUse(false);

        // Unequips the item 
        if (curItemProp.isEmpty) { bool x = RemoveFromInventory(curItem); }

        return isSuccess;
    }

    public bool Consume(int selectedSlot)
    {
        GameObject curItem = inventorySlots[selectedSlot][1];
        Item curItemProp = curItem.GetComponent<Item>();
        bool isSuccess = curItemProp.OnUse(false);

        // Unequips the item 
        if (curItemProp.isEmpty) { bool x = RemoveFromInventory(curItem); }

        return isSuccess;
    }



    // ========================================= THROW METHODS =========================================
    public bool Throw(int selectedSlot)
    {
        if (inventorySlots[selectedSlot][1] != null)
        {
            bool isSuccess = Throw(inventorySlots[selectedSlot][1]);
            return isSuccess;
        }
        else return false;
    }

    public bool Throw(GameObject curItem)
    {
        Item curItemProp = curItem.GetComponent<Item>();

        if (!curItemProp.isEmpty)
        {
            GameObject newDrop = Drop(1, new Vector2(0, 0), curItemProp.itemPrefab);
            newDrop.GetComponent<Item>().OverwriteStats(curItemProp.curQuantity, curItemProp.amountOverflow);
        }
        bool isSuccess = RemoveFromInventory(curItem);

        return isSuccess;
    }

    public void InventorySpill()
    {
        for(int i=0; i < inventorySlots.Count; i++)
        {
            bool x = Throw(i);
        }
    }
}
