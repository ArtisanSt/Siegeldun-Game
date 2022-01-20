using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactibles : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer sprite;

    public bool isItem { get; protected set; }
    protected bool _isIcon;

    public bool isSelected = false;
    private bool _curSelect = false;

    protected void ComponentInt()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ToggleInteraction(bool isSelected)
    {
        this.isSelected = isSelected;
        anim.SetBool("selected", isSelected);
    }

    protected void InteractibleUpdate()
    {
        if (!isItem || (isItem && !_isIcon))
        {
            if (_curSelect != isSelected)
            {
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
