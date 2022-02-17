using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : BaseDialogue
{
    [SerializeField] public Dialogue startingDialogue;
    [SerializeField] public Dialogue movementDialogue;
    [SerializeField] public Dialogue attackDialogue;


    // ================================================= DIALOGUE UPDATER =================================================
    protected void Update()
    {
        StartingDialogueSetter();
        MovementDialogueSetter();
        AttackDialogueSetter();

        MovementDialogueEnder();
        AttackDialogueEnder();
    }


    // ================================================= DIALOGUE STARTER =================================================
    private void StartingDialogueSetter()
    {
        dialogueSystem.PlayDialogue(startingDialogue);
    }

    private void MovementDialogueSetter()
    {
        if (startingDialogue.state != Dialogue.DialogueState.Done) return;

        dialogueSystem.PlayDialogue(movementDialogue);
    }

    private void AttackDialogueSetter()
    {
        if (startingDialogue.state != Dialogue.DialogueState.Done && movementDialogue.state != Dialogue.DialogueState.Done) return;

        dialogueSystem.PlayDialogue(attackDialogue);
    }


    // ================================================= DIALOGUE ENDER =================================================
    private void MovementDialogueEnder()
    {
        if (movementDialogue.state != Dialogue.DialogueState.Playing) return;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            dialogueSystem.EndDialogue();
        }
    }

    private void AttackDialogueEnder()
    {
        if (attackDialogue.state != Dialogue.DialogueState.Playing) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            dialogueSystem.EndDialogue();
        }
    }


    // ================================================= IDIALOGUE METHODS =================================================
    public override void OnEndMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue);
        base.OnEndMessage(curDialogue);
    }

    public override void OnStartMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue);
        base.OnStartMessage(curDialogue);
    }

    public override void OnDisplayMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue);
        base.OnDisplayMessage(curDialogue);
    }

    protected virtual void OverwriteDialogue(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref startingDialogue);
        OverwriteDialogue(curDialogue, ref movementDialogue);
        OverwriteDialogue(curDialogue, ref attackDialogue);
    }
}