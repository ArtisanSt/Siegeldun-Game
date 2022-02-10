using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    public Dialogue startingDialogue;
    public Dialogue movementDialogue;
    public Dialogue attackDialogue;

    void Update()
    {
        PlayDialogue(ref startingDialogue, true);
        PlayDialogue(ref movementDialogue, startingDialogue.isDone);
        PlayDialogue(ref attackDialogue, startingDialogue.isDone && movementDialogue.isDone);

        MovementDialogueConditions();
        AttackDialogueConditions();
    }

    private void PlayDialogue(ref Dialogue curDialogue, bool conditionals)
    {
        if (conditionals && !curDialogue.started)
        {
            curDialogue.started = true;
            GetComponent<DialogueSystem>().StartDialogue(curDialogue);
        }
    }

    public void EndDialogue(int dialoguesCount)
    {
        if (dialoguesCount == 0) { startingDialogue.isDone = true; }
        else if (dialoguesCount == 1) { movementDialogue.isDone = true; }
        else if (dialoguesCount == 2) { attackDialogue.isDone = true; }
    }

    private void MovementDialogueConditions()
    {
        if (movementDialogue.isDone || (!movementDialogue.isDone && !movementDialogue.started)) return;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            GetComponent<DialogueSystem>().EndDialogue();
            movementDialogue.isDone = true;
        }
    }

    private void AttackDialogueConditions()
    {
        if (attackDialogue.isDone || (!attackDialogue.isDone && !attackDialogue.started)) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GetComponent<DialogueSystem>().EndDialogue();
            attackDialogue.isDone = true;
        }
    }
}