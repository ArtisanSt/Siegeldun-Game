using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    // Inventory
    public GameObject[] slots = new GameObject[6];
    private GameObject playerEntity;
    public Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
    public bool[] isFull;
    private Item item;

    // Item Slot
    public string consumeSlot;
    public string weaponSlot;

    // Game Parameters
    private bool pickUp;
    private bool isOn;

    private void Start()
    {
        playerEntity = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(consumeSlot != null)
            {
                Consume(consumeSlot);
                RemoveItem(consumeSlot);   
            }
        }

        if(consumeSlot == null)
            Destroy(GameObject.Find("SlotB_Item"));

        pickUp = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Item"))
        {
            if(pickUp) return;

            AddItem(col.gameObject);

            pickUp = true;
        }
    }

    private void AddItem(GameObject item)
    {
        isOn = GameObject.Find("UI_System").GetComponent<UI_Events>().invOn;
        this.item = item.GetComponent<Item>();

        if(inventoryItems.ContainsKey(this.item.itemName))
        {
            inventoryItems[this.item.itemName] += 1;
            Destroy(item);
        }
        else
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if(isFull[i] == false)
                {
                    Debug.Log("Item " + this.item.itemName + " picked up!");
                    isFull[i] = true;

                    inventoryItems.Add(this.item.itemName, 1);

                    GameObject icon = (GameObject)Instantiate(this.item.itemPrefab, slots[i].transform, false);
                    icon.name = this.item.itemName + "_Prefab";
                    if(isOn)
                        icon.GetComponent<Image>().enabled = true;
                    else if(!isOn)
                        icon.GetComponent<Image>().enabled = false;

                    Destroy(item);
                    break;
                }
            }
        }
    }

    private void RemoveItem(string item)//GameObject item)
    {
        if(item == "") return;
        inventoryItems[item] -= 1;

        Debug.Log(inventoryItems[item]);
        if(inventoryItems[item] == 0)
        {
            inventoryItems.Remove(item);
            if(GameObject.Find(item + "_Prefab").transform.parent.name == "itemSlotA")
                isFull[0] = false;
            if(GameObject.Find(item + "_Prefab").transform.parent.name == "itemSlotB")
                isFull[1] = false;
            if(GameObject.Find(item + "_Prefab").transform.parent.name == "itemSlotC")
                isFull[2] = false;
            if(GameObject.Find(item + "_Prefab").transform.parent.name == "itemSlotD")
                isFull[3] = false;
            if(GameObject.Find(item + "_Prefab").transform.parent.name == "itemSlotE")
                isFull[4] = false;
            if(GameObject.Find(item + "_Prefab").transform.parent.name == "itemSlotF")
                isFull[5] = false;
            Destroy(GameObject.Find(item + "_Prefab"));

            consumeSlot = null;
        }

        Debug.Log(item + " used");
    }

    public void Consume(string consumable)
    {
        switch(consumable)
        {
            case "Health_Potion":
                playerEntity.GetComponent<Entity>().HpRegen(30f, "instant");
                Debug.Log("+30 HP!");
                break;
            case "Stamina_Potion":
                playerEntity.GetComponent<Entity>().StamRegen(30f, "instant");
                Debug.Log("+30 Stamina!");
                break;
        }
    }
}
