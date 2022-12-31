using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemProp : EntityProp
{
    public ItemProp()
    {
        entityType = EntityType.Item;
    }

    public string itemName { get { return entityName; } }
    public string itemTitle { get { return entityTitle; } }
    public string itemID { get { return entityID; } }

    // ============================== SHARED PROPERTIES ==============================
    public override string dirPath { get { return $"{base.dirPath}/Items/{itemType.ToString()}"; } }


    // ============================== MAIN PROPERTIES ==============================
    public enum ItemType { Weapon }
    public ItemType itemType;

    public int maxAmount;
}
