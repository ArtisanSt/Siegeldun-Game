using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject messageBox;
    private Queue<string> dialogue_messages;

    void Start()
    {
        dialogue_messages = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        messageBox.SetActive(true);
        nameText.text = dialogue.npc_name;
        dialogue_messages.Clear();

        foreach (string dialogue_message in dialogue.dialogue_messages)
        {
            dialogue_messages.Enqueue(dialogue_message);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(dialogue_messages.Count == 0)
        {
            EndDialogue();
            return;
        }

        string dialogue_message = dialogue_messages.Dequeue();
        dialogueText.text = dialogue_message;
    }

    void EndDialogue()
    {
        messageBox.SetActive(false);
    }
}
