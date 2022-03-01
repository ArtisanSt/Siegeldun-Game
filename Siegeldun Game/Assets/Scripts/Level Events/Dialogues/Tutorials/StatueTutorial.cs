using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueTutorial : BaseDialogue
{
    [SerializeField] public Dialogue statueDialogue;

    [SerializeField] public GameObject statue;

    // ================================================= DIALOGUE UPDATER =================================================
    protected void Update()
    {
        StatueDialogueEnder();
    }


    // ================================================= DIALOGUE STARTER =================================================
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != GameObject.Find("Player")) return;

        StatueDialogueSetter();
    }

    public void StatueDialogueSetter()
    {
        dialogueSystem.PlayDialogue(statueDialogue);
    }


    // ================================================= DIALOGUE ENDER =================================================
    private void StatueDialogueEnder()
    {

    }


    // ================================================= IDIALOGUE METHODS =================================================
    public override void OnEndMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref statueDialogue);
        base.OnEndMessage(curDialogue);
    }

    public override void OnStartMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref statueDialogue);
        base.OnStartMessage(curDialogue);
        if (statue.GetComponent<AchievementUnlocks>() != null) statue.GetComponent<AchievementUnlocks>().ChangeState();
    }

    public override void OnDisplayMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref statueDialogue);
        base.OnDisplayMessage(curDialogue);
    }
}
