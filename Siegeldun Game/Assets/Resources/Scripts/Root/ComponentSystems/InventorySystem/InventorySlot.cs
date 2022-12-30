using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InventorySlot<T> where T : ItemProp
{
   /* public string name { get; private set; }
    public Vector2Int amount { get; private set; }
    public T itemProp { get; private set; }

    public bool full { get { return amount.x == amount.y; } }

    // New Item
    public InventorySlot(Item<T> item)
    {
        this.name = item.nickname;
        this.amount = item.amount;
        this.itemProp = item.entityProp;
    }

    // returns overflow
    public int AddAmount(int amount)
    {
        this.amount.Set((this.amount.x + amount).Clamp(0,this.amount.y), this.amount.y);
        return (amount - this.amount.x).Positive();
    }

    // Returns if empty or not
    public bool RemoveAmount(int amount)
    {
        this.amount.Set((this.amount.x - amount).Clamp(0, this.amount.y), this.amount.y);
        return this.amount.x == 0;
    }*/
}
