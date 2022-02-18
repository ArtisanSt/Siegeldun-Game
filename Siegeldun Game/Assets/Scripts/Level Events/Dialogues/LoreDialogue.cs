using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreDialogue : BaseDialogue
{
    [SerializeField] public Dialogue loreDialogue;


    // ================================================= DIALOGUE UPDATER =================================================
    protected void Update()
    {
        LoreDialogueEnder();
    }


    // ================================================= DIALOGUE STARTER =================================================
    public void LoreDialogueSetter()
    {
        dialogueSystem.PlayDialogue(loreDialogue);
    }


    // ================================================= DIALOGUE ENDER =================================================
    private void LoreDialogueEnder()
    {

    }


    // ================================================= IDIALOGUE METHODS =================================================
    public override void OnEndMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref loreDialogue);
        base.OnEndMessage(curDialogue);
    }

    public override void OnStartMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref loreDialogue);
        base.OnStartMessage(curDialogue);
    }

    public override void OnDisplayMessage(Dialogue curDialogue)
    {
        OverwriteDialogue(curDialogue, ref loreDialogue);
        base.OnDisplayMessage(curDialogue);
    }
}
