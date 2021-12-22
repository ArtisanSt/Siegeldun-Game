using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public GameObject itemPrefab;
    public string itemName;
    public string itemType;
    public int amount = 0;

    public List<string> itemTypes = new List<string>()
    {
        "Consumable",
        "Weapon"
    };
}
