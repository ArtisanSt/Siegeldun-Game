using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string npcName;
    public bool hasButtons = false;
    public bool canGoBack = false;

    private bool _isDone = false;
    public bool isDone { get { return _isDone; } set { _isDone = value; } }
    private bool _started = false;
    public bool started { get { return _started; } set { _started = value; } }

    [TextArea(3, 10)]
    public List<string> dialogueMessages = new List<string>();
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] public Text nameText;
    [SerializeField] public Text dialogueText;
    [SerializeField] public GameObject messageBox;
    [SerializeField] public GameObject btnNext;
    [SerializeField] public GameObject btnBack;
    [SerializeField] public GameObject btnOK;
    private Dialogue dialogue;
    private int curIdx = 0;
    private int _dialoguesCount = -1;
    public int dialoguesCount { get { return _dialoguesCount; } private set { _dialoguesCount = value; } }

    public void ToggleButton(int phase) // -1: back, 1:next, 0:ok
    {
        if (phase != 0) DisplaySentence(curIdx + phase);
        else EndDialogue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue.dialogueMessages.Count == 0) return;

        messageBox.SetActive(true);
        this.dialogue = dialogue;
        nameText.text = dialogue.npcName;
        curIdx = 0;
        this.dialogue.started = true;
        dialoguesCount++;

        DisplaySentence(curIdx);
    }

    private void DisplaySentence(int curIdx)
    {
        if(dialogue.dialogueMessages.Count == curIdx)
        {
            EndDialogue();
            return;
        }

        this.curIdx = curIdx;
        SetDialogueButtons(curIdx);
        dialogueText.text = dialogue.dialogueMessages[curIdx];
    }

    private void SetDialogueButtons(int curIdx)
    {
        btnNext.SetActive(false);
        btnBack.SetActive(false);
        btnOK.SetActive(false);
        if (!dialogue.hasButtons) return;

        if (dialogue.dialogueMessages.Count == curIdx + 1)
        {
            btnOK.SetActive(true);
        }
        else
        {
            btnNext.SetActive(true);
        }

        if (curIdx != 0 && dialogue.canGoBack)
        {
            btnBack.SetActive(true);
        }
    }

    public void EndDialogue()
    {
        dialogue.isDone = true;
        messageBox.SetActive(false);
    }
}
