using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{/*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> inventorySlotsG;
    public int maxSlots { get { return inventorySlots.Count; } }

    public Dictionary<int, InventorySlot<ItemProp>> inventorySlots; // slotIndex, inventorySlot

    public void PropertyInit()
    {
        inventorySlots = new Dictionary<int, InventorySlot<ItemProp>>();
    }

    public bool AddItem<T>(Item<T> item) where T : ItemProp
    {
        if (item == null) return false;

        int overflow = 0;
        // Same Name Slots
        foreach (int slotIndex in SameNameSlots(item.nickname))
        {
            overflow = inventorySlots[slotIndex].AddAmount(item.amount.x);
            if (overflow == 0) break;
        }

        if (overflow == 0) return true;

        // Vacant Slots
        foreach (int slotIndex in VacantSlots())
        {
            inventorySlots[slotIndex] = new InventorySlot<T>(item);
            break;
        }

        //if (overflow > 0) CreateInstance();
        return true;
    }

    public int AddToSlot<T>(Item<T> item, int slotIndex) where T : ItemProp
    {
        int overflow = inventorySlots[slotIndex].AddAmount(item.amount.x);
        return overflow;
    }

    public int RemoveItem(int slotIndex, int amount, bool dropItem = true)
    {
        if (!inventorySlots.ContainsKey(slotIndex)) return amount;
        
        int overflow = (amount - inventorySlots[slotIndex].amount.x).Positive();
        bool empty = inventorySlots[slotIndex].RemoveAmount(amount);

        if (empty) inventorySlots.Remove(slotIndex);
        //if (dropItem) CreateInstance();
        return overflow;
    }

    public int RemoveItem(string itemName, int amount, bool dropItem = true)
    {
        int overflow = amount;
        foreach (int slotIndex in SameNameSlots(itemName, true))
        {
            overflow = RemoveItem(slotIndex, overflow, dropItem);
            if (overflow <= 0) break;
        }
        return overflow;
    }

    public int[] VacantSlots()
    {
        int[] output = new int[maxSlots - inventorySlots.Count];

        int x = 0;
        for (int i = 0; i < maxSlots; i++)
        {
            if (inventorySlots.ContainsKey(i)) continue;
            output[x] = i;
            x++;

            if (x == output.Length) break;
        }

        return output;
    }

    public int[] SameNameSlots(string name, bool includeFull = false)
    {
        List<int> output = new List<int>();

        foreach (KeyValuePair<int, InventorySlot<ItemProp>> inventorySlot in inventorySlots)
        {
            if (inventorySlot.Value.name == name)
            {
                if (!includeFull && inventorySlot.Value.full) continue;

                output.Add(inventorySlot.Key);
            }
        }

        return output.ToArray();
    }*/
}
