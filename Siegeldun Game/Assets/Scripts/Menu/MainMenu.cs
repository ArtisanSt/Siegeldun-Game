using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Singleton<MainMenu>
{
    private GameMechanics gameMechanics;
    [SerializeField] private GameObject btnContinue;

    protected override void Awake()
    {
        base.Awake();
        SaveAndLoadManager.LoadLevelData();
        btnContinue.SetActive(GlobalVariableStorage.curAct != 0 || GlobalVariableStorage.curLvl != 0);
        for (int i=0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(child.name != "FloatingTabs");
        }
        gameMechanics = GameObject.Find("Game System/GameMechanics").GetComponent<GameMechanics>();
        GameMechanics.gameState = GameMechanics.GameState.MainMenu;
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(true);
            GlobalVariableStorage.achievements = new bool[4] { false, false, false, false };
            GlobalVariableStorage.hostiles = new bool[4] { false, false, false, false };
            GlobalVariableStorage.potions = new bool[2] { false, false };
            GlobalVariableStorage.weapons = new bool[1] { false };
            SaveAndLoadManager.SaveGameData();
        }
    }*/

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void StartNewGame(string difficulty)
    {
        gameMechanics.StartNewGame(difficulty);
    }

    public void ContinueGame()
    {
        gameMechanics.ContinueGame();
    }
}
