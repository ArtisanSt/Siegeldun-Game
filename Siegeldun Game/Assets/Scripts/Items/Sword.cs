using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    void Start()
    {
        itemName = "Sword";
        itemType = itemTypes[1];
    }
}