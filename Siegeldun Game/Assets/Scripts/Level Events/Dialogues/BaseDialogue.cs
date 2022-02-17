using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDialogue : MonoBehaviour, IDialogue
{
    protected static DialogueSystem dialogueSystem;

    protected void Awake()
    {
        if (dialogueSystem == null) dialogueSystem = GameObject.Find("Level System").GetComponent<DialogueSystem>();
    }

    // ================================================= IDIALOGUE METHODS =================================================
    public virtual void OnEndMessage(Dialogue curDialogue)
    {
        if (GetComponent<IInteractible>() != null && curDialogue.repeatable) GetComponent<IInteractible>().ToggleInteractible();  // turns the interactible ability to on
    }

    public virtual void OnStartMessage(Dialogue curDialogue)
    {
        if (GetComponent<IInteractible>() != null) GetComponent<IInteractible>().ToggleInteractible(); // turns the interactible ability to off
    }

    public virtual void OnDisplayMessage(Dialogue curDialogue)
    {

    }

    protected void OverwriteDialogue(Dialogue curDialogue, ref Dialogue refDialogue)
    {
        if (curDialogue.messageTitle == refDialogue.messageTitle)
        {
            refDialogue = curDialogue;
        }
    }
}
