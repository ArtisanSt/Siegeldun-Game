using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    public Dialogue movementDialogue;
    public Dialogue attackDialogue;
    private bool movementTutorialDone = false;
    private bool battleTutorialDone = false;

    void Start()
    {
        this.gameObject.GetComponent<DialogueTrigger>().TiggerDialogue();
    }

    void Update()
    {
        if(!movementTutorialDone)
        {
            if(Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                FindObjectOfType<DialogueSystem>().StartDialogue(movementDialogue);
                movementTutorialDone = true;
            }
        }

        if(movementTutorialDone)
        {
            if(!battleTutorialDone)
            {
                if(Input.GetKeyDown(KeyCode.Mouse1))
                {
                    FindObjectOfType<DialogueSystem>().StartDialogue(attackDialogue);
                    battleTutorialDone = true;
                }
            }
        }
    }
}