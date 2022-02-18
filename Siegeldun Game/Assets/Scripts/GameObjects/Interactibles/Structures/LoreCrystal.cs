using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreCrystal : Structures
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected) return;

        GetComponent<LoreDialogue>().LoreDialogueSetter();
    }
}
