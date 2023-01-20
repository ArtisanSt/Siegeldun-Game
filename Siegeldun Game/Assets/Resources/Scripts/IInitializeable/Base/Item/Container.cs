using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Container
{
    public AmountInt amount;
    public List<ItemProp> items;

    public Container()
    {
        items = new List<ItemProp>();
    }

    public void Init()
    {
        if (items.Count > amount.max)
            items = items.GetRange(0, amount.max);
        else if (items.Count < amount.max)
        {
            for (int i=items.Count; i<amount.max; i++)
            {
                items.Add(new ItemProp());
            }
        }
    }

    public bool AddItem(ItemProp itemProp)
    {
        return true;
    }

    public bool RemoveItem(ItemProp itemProp)
    {
        return true;
    }

    public bool RemoveItem(int index)
    {
        return true;
    }

    private void SetToNull(int index)
    {
        items[index] = NullProp();
    }

    private ItemProp NullProp()
    {
        ItemProp temp = new ItemProp();

        temp.name = "NULL";
        temp.title = "NULL";
        temp.ID = "NULL";
        temp.amount = new AmountInt(0,0);
        temp.itemType = ItemProp.ItemType.Null;

        return temp;
    }
}