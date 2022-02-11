using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : DialogueSystem
{
    [SerializeField] public GameObject player;

    [SerializeField] public Dialogue startingDialogue;
    [SerializeField] public Dialogue movementDialogue;
    [SerializeField] public Dialogue attackDialogue;
    [SerializeField] public Dialogue statueDialogue;
    private Vector2 center;
    private Vector2 size;

    protected void Awake()
    {
        player = GameObject.Find("Player");
    }

    protected override void Update()
    {
        base.Update();

        PlayDialogue(ref startingDialogue, true);
        PlayDialogue(ref movementDialogue, MovementDialogueSetter());
        PlayDialogue(ref attackDialogue, AttackDialogueSetter());
        PlayDialogue(ref statueDialogue, StatueDialogueSetter());

        DialogueConditions();
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

    private bool StatueDialogueSetter()
    {
        if (statueDialogue.started) return false;

        Interactor playerInteractor = player.GetComponent<Interactor>();
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerInteractor.center, playerInteractor.size, 0, LayerMask.GetMask("Structures"));
        if (hitColliders.Length == 0) return false;

        bool found = false;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject curObject = hitColliders[i].gameObject;
            if (curObject.GetComponent<IStructure>().structureName == "Statue")
            {
                found = true;
                break;
            }
        }

        return found && startingDialogue.isDone && movementDialogue.isDone && attackDialogue.isDone;
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