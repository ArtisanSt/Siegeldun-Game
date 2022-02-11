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

        PlayDialogue(ref startingDialogue, true);
        PlayDialogue(ref movementDialogue, startingDialogue.isDone);
        PlayDialogue(ref attackDialogue, startingDialogue.isDone && movementDialogue.isDone);

        MovementDialogueConditions();
        AttackDialogueConditions();
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