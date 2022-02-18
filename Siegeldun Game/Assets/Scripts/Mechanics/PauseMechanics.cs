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

    void Awake()
    {
        instance = this;
        gameMechanics = GameObject.Find("Game System/GameMechanics").GetComponent<GameMechanics>();
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
        SetPlayTime(true);
        gameMechanics.BackToMainMenu();
    }
}
