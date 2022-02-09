using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    Transform slotA, slotB;
    public GameObject portal;

    void Start()
    {
        slotA = transform.Find("SlotA");
        slotB = transform.Find("SlotB");
    }

    // Update is called once per frame
    void Update()
    {
        if(slotA.GetComponent<GateSlot>().slotted && slotB.GetComponent<GateSlot>().slotted)
        {
            portal.SetActive(true);
        }
    }
}