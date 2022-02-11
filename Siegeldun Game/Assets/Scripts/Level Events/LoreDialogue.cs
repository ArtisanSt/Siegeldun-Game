using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreDialogue : DialogueSystem
{
    [SerializeField] public Dialogue loreDialogue;

    protected override void Update()
    {
        base.Update();

        LoreDialogueConditions();
    }

    public void StartDialogue()
    {
        PlayDialogue(ref loreDialogue, true);
    }

    // Will only execute if the loreDialuge starts showing
    private void LoreDialogueConditions()
    {
        if (loreDialogue.isDone || (!loreDialogue.isDone && !loreDialogue.started)) return;
    }
}
