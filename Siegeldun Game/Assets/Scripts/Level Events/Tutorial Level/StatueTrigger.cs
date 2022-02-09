using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueTrigger : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    private bool statueTutorial = false;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(!statueTutorial)
        {
            FindObjectOfType<DialogueSystem>().StartDialogue(dialogue);
            statueTutorial = true;
        }
    }
}
