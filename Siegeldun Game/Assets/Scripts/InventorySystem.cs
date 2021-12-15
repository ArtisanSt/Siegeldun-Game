using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // ========================================= ENTITY INVENTORY =========================================
    // Inventory Mechanics
    public List<string> inventory;
    private bool pickUp;

    void Start()
    {
        inventory = new List<string>();
    }

    void Update()
    {
        // Pick Up Reset - Multiple colliders on player causes multiple item pick ups
        pickUp = false;
    }

    // Item Pickup Method
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            // Algorithm to pickup only once due to multiple player colliders
            if(pickUp) return;
            pickUp = true;

            // Get Item Type
            string item = collision.gameObject.GetComponent<Item>().itemType;

            // Store Item to Inventory
            inventory.Add(item);
            Debug.Log("Item Collected " + item);// + item);
            Destroy(collision.gameObject);
        }
    }
}
