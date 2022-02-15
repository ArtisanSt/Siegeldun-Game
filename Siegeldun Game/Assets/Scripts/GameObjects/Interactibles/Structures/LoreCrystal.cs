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
        if (!GetComponent<LoreDialogue>().loreDialogue.repeatable && GetComponent<LoreDialogue>().loreDialogue.isDone)
        {
            GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
            isInteractible = false;
        }
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected) return;

        GetComponent<LoreDialogue>().StartDialogue();
    }
}
