using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public GameObject messageSource;

    public bool hasButtons = false;
    public bool repeatable = false;
    public bool canGoBack = false;
    public bool isTimed = false;
    public float timer = 0;

    public enum DialogueState { StandBy, Playing, Done}
    private DialogueState _state = DialogueState.StandBy;
    public DialogueState state { get { return _state; } set { _state = value; } }

    public string messageTitle;
    public string npcName;
    [TextArea(3, 10)]
    public List<string> dialogueMessages = new List<string>();

    public Transform entityReference;
    public float maxDistanceToReference;
}

public class DialogueSystem : Process
{
    private static GameObject messageBox;
    private static Image dialogueBG;
    private static Text nameText;
    private static Text dialogueText;
    private static GameObject btnNext;
    private static GameObject btnBack;

    private static Transform player;

    private static Dialogue dialogue;
    private static int curIdx = -1;
    private static bool isPlaying = false;
    private static float msgStart = 0f; // Time.time when message started playing

    private static bool autoTimed;
    private static float autoTimer = 5f; // Messages with no timer should only last for 10 seconds

    private static bool hasReference;
    private static Transform referencedEntity; // Messages that are referenced to an entity will have limitations
    private static float distanceToReference; // Distance Limitation to a referenced entity

    void Awake()
    {
        messageBox = (messageBox == null) ? GameObject.Find("/GUI/MessageBox") : messageBox;
        dialogueBG = (dialogueBG == null) ? messageBox.transform.GetChild(0).GetComponent<Image>() : dialogueBG;
        nameText = (nameText == null) ? messageBox.transform.GetChild(1).GetComponent<Text>() : nameText;
        dialogueText = (dialogueText == null) ? messageBox.transform.GetChild(2).GetComponent<Text>() : dialogueText;
        btnNext = (btnNext == null) ? messageBox.transform.GetChild(3).gameObject : btnNext;
        btnBack = (btnBack == null) ? messageBox.transform.GetChild(4).gameObject : btnBack;

        if (btnNext != null) btnNext.GetComponent<Button>().onClick.AddListener(() => DisplayChangeDialogue(1));
        if (btnBack != null) btnBack.GetComponent<Button>().onClick.AddListener(() => DisplayChangeDialogue(-1));

        player = (player == null) ? GameObject.Find("Player").transform : player;
    }

    void Start()
    {
        btnNext.SetActive(false);
        btnBack.SetActive(false);
        if (messageBox.activeSelf) messageBox.SetActive(false);
    }

    private void DialogueStopper()
    {
        if (!PauseMechanics.isPlaying && !isPlaying && dialogue == null) return;

        // Check if dialogue system is being used
        bool timerDone = dialogue.isTimed && TimerIncrement(msgStart, dialogue.timer);
        bool noBtnAutoTimed = autoTimed && TimerIncrement(msgStart, autoTimer);
        if (timerDone || noBtnAutoTimed)
        {
            DisplayChangeDialogue(1);
        }

        if (hasReference)
        {
            distanceToReference = Mathf.Abs(player.position.x - referencedEntity.position.x);
            if (dialogue.maxDistanceToReference < distanceToReference) EndDialogue();
        }
    }

    private bool CanPlayMessage(Dialogue curDialogue)
    {
        return curDialogue != null && curDialogue.dialogueMessages.Count > 0 && curDialogue.state == Dialogue.DialogueState.StandBy && !isPlaying;
    }

    // Will only Trigger Once to start the message
    public void PlayDialogue(Dialogue curDialogue)
    {
        if (!CanPlayMessage(curDialogue)) return;

        if (curDialogue.messageSource.GetComponent<IDialogue>() != null) curDialogue.messageSource.GetComponent<IDialogue>().OnStartMessage(curDialogue);

        dialogue = curDialogue;

        // Setting up Referenced Entity
        referencedEntity = dialogue.entityReference;
        hasReference = referencedEntity != null;

        // Setting up Auto-timers
        autoTimed = !dialogue.hasButtons && !dialogue.isTimed;

        // Setting up Message Properties
        nameText.text = dialogue.npcName;
        curIdx = -1;

        // Showing the messages
        messageBox.SetActive(true);
        isPlaying = true;
        dialogue.state = Dialogue.DialogueState.Playing;

        InvokeRepeating("DialogueStopper", 0, 0.1f);

        DisplayChangeDialogue(1);
    }

    public void DisplayChangeDialogue(int idxChange)
    {
        if (!isPlaying) return;

        curIdx += idxChange;
        if (dialogue.dialogueMessages.Count == curIdx || curIdx < 0)
        {
            if (dialogue.dialogueMessages.Count == curIdx) EndDialogue();
            return;
        }

        msgStart = Time.time;
        SetDialogueButtons();
        if (dialogue.messageSource.GetComponent<IDialogue>() != null) dialogue.messageSource.GetComponent<IDialogue>().OnDisplayMessage(dialogue);
        dialogueText.text = dialogue.dialogueMessages[curIdx];
    }

    private void SetDialogueButtons()
    {
        btnNext.SetActive(dialogue.hasButtons);
        if (!dialogue.hasButtons) return;

        btnNext.transform.GetChild(0).GetComponent<Text>().text = (dialogue.dialogueMessages.Count == curIdx + 1) ? "OK" : "Next";
        btnBack.SetActive(curIdx != 0 && dialogue.canGoBack);
    }

    public void EndDialogue()
    {
        dialogue.state = (dialogue.repeatable) ? Dialogue.DialogueState.StandBy : Dialogue.DialogueState.Done;
        if (dialogue.messageSource != null && dialogue.messageSource.GetComponent<IDialogue>() != null) dialogue.messageSource.GetComponent<IDialogue>().OnEndMessage(dialogue);

        CancelInvoke("DialogueStopper");
        messageBox.SetActive(false);
        isPlaying = false;
        dialogue = null;
    }
}
