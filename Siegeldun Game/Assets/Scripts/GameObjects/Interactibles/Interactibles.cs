using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactibles : BaseObject, IInteractible
{
    // Interacting Properties
    [Header("INTERACTIBLE SETTINGS", order = 0)]
    [SerializeField] private bool _isInteractible;
    public bool isInteractible { get { return _isInteractible; } protected set { _isInteractible = value; } }
    protected bool disableInteract = false;

    public string objectClassification { get; protected set; } // "ICON", "ITEM", "STRUCTURE"

    private bool _isSelected = false; 
    public bool isSelected { get { return _isSelected; } protected set { _isSelected = value; } }
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

    public void ToggleInteractible()
    {
        isInteractible = !isInteractible;
        if (!isInteractible)
        {
            GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
        }
    }

    public void DisableInteract()
    {
        disableInteract = true;
    }

    public virtual void Interact() { }
}
