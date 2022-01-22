using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanics : SingletonDontDestroy<GameMechanics>
{
    // ========================================= GAME MECHANICS PROPERTIES =========================================
    // Game Properties
    public Dictionary<string, LevelProperties> allLvlProps { get; private set; }

    private string LevelPropertyName(int actNumber, int levelNumber)
    {
        return $"{actNumber.ToString()}_{levelNumber.ToString()}";
    }

    //public LevelProperties curLvlProp { get; private set; }
    public LevelProperties curLvlProp = new LevelProperties(); // Pseudo

    public bool lvlDone { get; private set; } // current level is done
    public bool gameDone { get; private set; } // Final Level is Done

    // Player Global Properties
    public GameObject playerGameObject { get; private set; }
    public bool playerAlive { get; private set; }



    // ========================================= UNITY METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        gameObject.name = "System";
        allLvlProps = new Dictionary<string, LevelProperties>();
        if (curLvlProp == null)
        {
            GameInit();
        }

        playerGameObject = GameObject.Find("Player");
    }

    void Update()
    {
        // playerAlive = (playerGameObject == null) ? false : playerGameObject.GetComponent<Entity>().isAlive;
    }


    // ========================================= GAME MECHANICS METHODS =========================================
    // Calls when transitioned from main menu to game
    public void GameInit(int gameDifficulty = 0, int actNumber = 0, int levelNumber = 0)
    {
        GlobalVariableStorage.gameDifficulty = gameDifficulty;
        curLvlProp = new LevelProperties(gameDifficulty, actNumber, levelNumber);
        SaveStartingLvlProp();
        lvlDone = false;
    }

    public void LevelDone()
    {
        // Conditions to check if the level is really done
        lvlDone = true;


        if (lvlDone)
        {
            // Stop every summoning entity
            // Unload current Scene and reload next
            // gameDone = curLvlProp.LevelFinished();
            // if (gameDone) {// Game Over}
            // else:
            // SaveStartingLvlProp();
        }
    }

    // Pseudo Saving Level Defaults Mechanics
    public void SaveStartingLvlProp()
    {
        LevelProperties defaultLvlProp = new LevelProperties();
        defaultLvlProp.CloneProperties(curLvlProp);
        allLvlProps.Add(LevelPropertyName(defaultLvlProp.actNumber, defaultLvlProp.levelNumber), defaultLvlProp);
    }
}
