using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string npcName;
    public bool hasButtons = false;
    public bool repeatable = false;
    public bool canGoBack = false;
    public bool isTimed = false;
    public int timer = 0;

    private bool _isDone = false;
    public bool isDone { get { return _isDone; } set { _isDone = value; } }
    private bool _started = false;
    public bool started { get { return _started; } set { _started = value; } }

    public string messageTitle;
    [TextArea(3, 10)]
    public List<string> dialogueMessages = new List<string>();
}

public abstract class DialogueSystem : Process
{
    [SerializeField] public GameObject messageBox;
    [SerializeField] public Image dialogueBG;
    [SerializeField] public Text nameText;
    [SerializeField] public Text dialogueText;
    [SerializeField] public GameObject btnNext;
    [SerializeField] public GameObject btnBack;
    private static Dialogue dialogue;
    private static int curIdx = -1;
    private static bool isPlaying = false;
    private static float msgStart = 0f;
    protected static string curMsg = "";

    protected virtual void Awake()
    {
        messageBox = (messageBox == null) ? GameObject.Find("/GUI/MessageBox") : messageBox;
        dialogueBG = (dialogueBG == null) ? messageBox.transform.GetChild(0).GetComponent<Image>() : dialogueBG;
        nameText = (nameText == null) ? messageBox.transform.GetChild(1).GetComponent<Text>() : nameText;
        dialogueText = (dialogueText == null) ? messageBox.transform.GetChild(2).GetComponent<Text>() : dialogueText;
        btnNext = (btnNext == null) ? messageBox.transform.GetChild(3).gameObject : btnNext;
        btnBack = (btnBack == null) ? messageBox.transform.GetChild(4).gameObject : btnBack;

        if (btnNext != null) btnNext.GetComponent<Button>().onClick.AddListener(() => DisplayChangeDialogue(1));
        if (btnBack != null) btnBack.GetComponent<Button>().onClick.AddListener(() => DisplayChangeDialogue(-1));

        // Component Initializer to avoid future errors
        if (messageBox != null)
        {
            dialogueBG = messageBox.transform.GetChild(0).GetComponent<Image>();
            nameText = messageBox.transform.GetChild(1).GetComponent<Text>();
            dialogueText = messageBox.transform.GetChild(2).GetComponent<Text>();
            btnNext = messageBox.transform.GetChild(3).gameObject;
            btnBack = messageBox.transform.GetChild(4).gameObject;
        }

        btnNext.GetComponent<Button>().onClick.AddListener(() => DisplayChangeDialogue(1));
        btnBack.GetComponent<Button>().onClick.AddListener(() => DisplayChangeDialogue(-1));
    }

    protected virtual void Start()
    {
        if (messageBox.activeSelf) messageBox.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!PauseMechanics.isPlaying) return;

        bool timerDone = dialogue != null && isPlaying && dialogue.isTimed && TimerIncrement(msgStart, dialogue.timer);
        bool noBtnAutoTimed = dialogue != null && isPlaying && !dialogue.isTimed && !dialogue.hasButtons && TimerIncrement(msgStart, 3);
        bool msgStayedTooLong = dialogue != null && isPlaying && TimerIncrement(msgStart, 10);
        if (timerDone || noBtnAutoTimed || msgStayedTooLong)
        {
            DisplayChangeDialogue(1);
        }
    }

    public void StartDialogue(ref Dialogue curDialogue)
    {
        if (curDialogue == null || curDialogue.dialogueMessages.Count == 0 || isPlaying) return;

        messageBox.SetActive(true);
        dialogue = curDialogue;
        nameText.text = dialogue.npcName;
        curIdx = -1;
        isPlaying = true;
        curMsg = dialogue.messageTitle;

        DisplayChangeDialogue(1);
    }

    public void DisplayChangeDialogue(int idxChange)
    {
        if (dialogue == null || !isPlaying || dialogue.dialogueMessages.Count == 0) return;

        curIdx += idxChange;
        if (dialogue.dialogueMessages.Count == curIdx || curIdx < 0)
        {
            if (dialogue.dialogueMessages.Count == curIdx) EndDialogue();
            return;
        }

        msgStart = Time.time;
        SetDialogueButtons();
        dialogueText.text = dialogue.dialogueMessages[curIdx];
    }

    private void SetDialogueButtons()
    {
        btnNext.SetActive(false);
        btnBack.SetActive(false);
        if (!dialogue.hasButtons) return;

        // Dialogue is the last one
        btnNext.SetActive(true);
        if (dialogue.dialogueMessages.Count == curIdx + 1)
        {
            btnNext.transform.GetChild(0).GetComponent<Text>().text = "OK";
        }
        else
        {
            btnNext.transform.GetChild(0).GetComponent<Text>().text = "Next";
        }

        if (curIdx != 0 && dialogue.canGoBack)
        {
            btnBack.SetActive(true);
        }
    }

    public void EndDialogue()
    {
        if (dialogue.repeatable)
        {
            dialogue.started = false;
            ToggleInteractible();
        }
        else { dialogue.isDone = true; }
        isPlaying = false;
        curMsg = "";
        dialogue = null;
        messageBox.SetActive(false);
    }

    // Will only Trigger Once to start the message
    public void PlayDialogue(ref Dialogue curDialogue, bool conditionals)
    {
        if (conditionals && !curDialogue.started && !isPlaying)
        {
            curDialogue.started = true;
            ToggleInteractible();
            StartDialogue(ref curDialogue);
        }
    }

    private void ToggleInteractible()
    {
        if (GetComponent<IInteractible>() != null) GetComponent<IInteractible>().ToggleInteractible();
    }
}
