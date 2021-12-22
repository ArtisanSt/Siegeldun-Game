using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Events : MonoBehaviour
{
    GameObject mainInventory; // = GameObject.Find("Inventory Background");
    public bool invOn;

    private void Start()
    {
        mainInventory = GameObject.Find("Inventory Background");
        if(mainInventory.GetComponent<Image>().enabled == true)
            invOn = true;
        else
            invOn = false;
    }

    public void OffMainInventory()
    {
        mainInventory.GetComponent<Image>().enabled = false;
        for(int i = 0; i <= 5; i++)
        {
            Transform kid = mainInventory.transform.GetChild(i);
            kid.GetComponent<Image>().enabled = false;
            foreach(Transform child in kid)
            {
                child.GetComponent<Image>().enabled = false;
            }
        }
        invOn = false;
    }

    public void OnMainInventory()
    {
        mainInventory.GetComponent<Image>().enabled = true;
        for(int i = 0; i <= 5; i++)
        {
            Transform kid = mainInventory.transform.GetChild(i);
            kid.GetComponent<Image>().enabled = true;
            foreach(Transform child in kid)
            {
                child.GetComponent<Image>().enabled = true;
            }
        }
        invOn = true;
    }
}
