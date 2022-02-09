using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Crystal : Key
{
    // ========================================= Item Initialization =========================================
    protected void ItemInit()
    {
        objectName = "Key_Crystal";
        itemName = "Crystal";

        maxQuantity = 2;
        curQuantity = 1;
    }

    // ========================================= UNITY MAIN METHODS =========================================
    void Awake()
    {
        InteractInit();
        ItemInit();
        KeyInit();
    }

    void Start()
    {
        PrefabsInit();
    }
}
