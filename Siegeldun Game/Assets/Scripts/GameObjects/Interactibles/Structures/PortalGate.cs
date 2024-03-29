using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGate : Structures
{
    [SerializeField] public List<Transform> gateSlots = new List<Transform>();
    [SerializeField] public GameObject portal;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        OpenPortal();
    }

    protected bool IsAllGateSlotsTaken()
    {
        bool output = true;
        foreach (Transform gateSlot in gateSlots)
        {
            if (!gateSlot.GetComponent<IGateSlot>().slotted)
            {
                output = false;
                break;
            }
        }
        return output;
    }

    protected void OpenPortal()
    {
        if (IsAllGateSlotsTaken() && !portal.activeSelf) { portal.SetActive(true); }
    }
}