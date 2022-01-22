using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactibles : Root
{
    // Interacting Properties
    public bool isInteractible { get; protected set; }

    public string objectClassification { get; protected set; } // "ICON", "ITEM", "STRUCTURE"

    public bool isSelected { get; protected set; }
    private bool _curSelect = false;

    public void ToggleInteract(bool isSelected)
    {
        if (isInteractible)
        {
            if (_curSelect != isSelected)
            {
                this.isSelected = isSelected;
                _curSelect = isSelected;
                if (isSelected)
                {
                    GetComponent<SpriteRenderer>().material.EnableKeyword("OUTLINE_ON");
                }
                else
                {
                    GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
                }
            }
        }
    }
}
