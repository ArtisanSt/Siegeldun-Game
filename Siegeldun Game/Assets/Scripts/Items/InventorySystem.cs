using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private bool pickUp;
    public List<string> inventory;

    private Inventory playerInventory;
    private Item item;
    public GameObject itemButton;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

    }

    private void Update()
    {
        // Pick Up Reset - Multiple colliders on player causes multiple item pick ups
        pickUp = false;
    }

    // Item Pickup Method
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            item = collision.gameObject.GetComponent<Item>();
            if(pickUp) return;
            pickUp = true;

            for (int i = 0; i < playerInventory.slots.Length; i++)
            {
                if (playerInventory.isFull[i] == false)
                {
                    playerInventory.isFull[i] = true;
                    inventory.Add(item.itemType);

                    Instantiate(itemButton, playerInventory.slots[i].transform, false);
                    Destroy(collision.gameObject);
                    break;
                }
            }
        }
    }
}
