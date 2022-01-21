using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactibles : MonoBehaviour
{
    // Interacting Properties
    public bool canInteract { get; protected set; }
    public bool isItem { get; protected set; }
    public bool isIcon { get; protected set; }

    public bool isSelected { get; protected set; }
    private bool _curSelect = false;

    public void ToggleInteract(bool isSelected)
    {
        if (canInteract)
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
