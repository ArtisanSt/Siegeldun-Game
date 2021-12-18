using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    void Start()
    {
        itemName = "Health_Potion";
        itemType = itemTypes[0];
    }
}