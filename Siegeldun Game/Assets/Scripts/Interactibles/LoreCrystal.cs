using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreCrystal : Interactibles
{
    void Awake()
    {
        isItem = false;
        isIcon = false;
        canInteract = true;
        isSelected = false;
    }

    void Update()
    {
        Interaction();
    }

    // Interaction Event
    void Interaction()
    {
        if(isSelected)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                this.gameObject.GetComponent<DialogueTrigger>().TiggerDialogue();
            }
        }
    }
}
