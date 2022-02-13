using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : DialogueSystem
{
    [SerializeField] public Dialogue startingDialogue;
    [SerializeField] public Dialogue movementDialogue;
    [SerializeField] public Dialogue attackDialogue;

    protected override void Update()
    {
        base.Update();

        PlayDialogue(ref startingDialogue, StartingDialogueSetter());
        PlayDialogue(ref movementDialogue, MovementDialogueSetter());
        PlayDialogue(ref attackDialogue, AttackDialogueSetter());

        DialogueConditions();
    }


    private bool StartingDialogueSetter()
    {
        if (movementDialogue.started) return false;

        return true;
    }

    private bool MovementDialogueSetter()
    {
        if (movementDialogue.started) return false;

        return startingDialogue.isDone;
    }

    private bool AttackDialogueSetter()
    {
        if (attackDialogue.started) return false;

        return startingDialogue.isDone && movementDialogue.isDone;
    }



    private void DialogueConditions()
    {
        if (curMsg == movementDialogue.messageTitle) MovementDialogueConditions();
        else if (curMsg == attackDialogue.messageTitle) AttackDialogueConditions();
    }

    private void MovementDialogueConditions()
    {
        if (movementDialogue.isDone || (!movementDialogue.isDone && !movementDialogue.started)) return;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            EndDialogue();
            movementDialogue.isDone = true;
        }
    }

    private void AttackDialogueConditions()
    {
        if (attackDialogue.isDone || (!attackDialogue.isDone && !attackDialogue.started)) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            EndDialogue();
            attackDialogue.isDone = true;
        }
    }
}