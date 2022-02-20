using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMechanics : MonoBehaviour
{
    public static PauseMechanics instance;
    [SerializeField] public GameMechanics gameMechanics;

    public static bool isPlaying = true;
    private static bool _isPlaying = false;
    [SerializeField] public GameObject PauseMenuUI;

    [Header("Button Settings")]
    [SerializeField] public GameObject settings;
    [SerializeField] public RectTransform btnSettings;
    private GameMechanics.GameState curState = GameMechanics.GameState.MainMenu;
    [SerializeField] public Vector3 mainMenuPosition;
    [SerializeField] public Vector3 inGamePosition;

    [Header("Pause and Settings")]
    [SerializeField] public TMPro.TextMeshProUGUI textTitle;
    [SerializeField] public GameObject btnResume;

    [SerializeField] public RectTransform btnMenuBack;
    [SerializeField] public Vector3 btnBackPosition;
    [SerializeField] public Vector3 btnMenuPosition;

    [SerializeField] public RectTransform bgmSetting;
    [SerializeField] public Vector3 btnBgmMmPosition;
    [SerializeField] public Vector3 btnBgmIgPosition;

    [SerializeField] public RectTransform sfxSetting;
    [SerializeField] public Vector3 btnSfxMmPosition;
    [SerializeField] public Vector3 btnSfxIgPosition;


    void Awake()
    {
        instance = this;
        gameMechanics = GameObject.Find("Game System/GameMechanics").GetComponent<GameMechanics>();
        SetButtonSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenuUI == null || GameMechanics.gameState != GameMechanics.GameState.InGame) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPlaying = !isPlaying;
        }

        if (isPlaying != _isPlaying)
        {
            SetPlayTime(isPlaying);
        }

        if (curState != GameMechanics.gameState)
        {
            SetButtonSettings();
        }
    }

    private void SetButtonSettings()
    {
        curState = GameMechanics.gameState;
        btnSettings.localPosition = (curState == GameMechanics.GameState.MainMenu) ? mainMenuPosition : inGamePosition;

        textTitle.text = (curState == GameMechanics.GameState.InGame) ? "PAUSED" : "SETTINGS";
        btnResume.SetActive(curState == GameMechanics.GameState.InGame);
        btnMenuBack.localPosition = (curState == GameMechanics.GameState.MainMenu) ? btnBackPosition : btnMenuPosition;
        bgmSetting.localPosition = (curState == GameMechanics.GameState.MainMenu) ? btnBgmMmPosition : btnBgmIgPosition;
        sfxSetting.localPosition = (curState == GameMechanics.GameState.MainMenu) ? btnSfxMmPosition : btnSfxIgPosition;
    }

    public void SetPlayTime(bool value)
    {
        SetPlayTime(value, true);
    }

    public void SetPlayTime(bool value, bool changeUI)
    {
        isPlaying = value;
        _isPlaying = value;
        if (changeUI) PauseMenuUI.SetActive(!isPlaying);
        Time.timeScale = (isPlaying) ? 1f : 0f;
    }

    public void TogglePlayTime()
    {
        SetPlayTime(!isPlaying);
    }

    public void MoveToMainMenu()
    {
        if (curState == GameMechanics.GameState.MainMenu)
        {
            PauseMenuUI.SetActive(false);
        }
        else
        {
            SetPlayTime(true);
            gameMechanics.BackToMainMenu();
        }
    }

    public void BntSettingsSetActive(bool isActive)
    {
        settings.SetActive(isActive);
    }
}
