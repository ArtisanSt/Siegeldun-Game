using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Singleton<MainMenu>
{
    private GameMechanics gameMechanics;

    protected override void Awake()
    {
        base.Awake();
        for (int i=0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(child.name != "FloatingTabs");
        }
        gameMechanics = GameObject.Find("Game System/GameMechanics").GetComponent<GameMechanics>();
        GameMechanics.gameState = GameMechanics.GameState.MainMenu;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void StartNewGame(string difficulty)
    {
        gameMechanics.StartNewGame(difficulty);
    }
}
