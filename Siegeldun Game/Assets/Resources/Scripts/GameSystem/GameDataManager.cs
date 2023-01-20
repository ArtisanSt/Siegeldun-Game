using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameDataManager
{
    public static GameData CreateGameData(string dataName, int difficulty, int act=0, int segment=0)
    {
        GameData gameData = new GameData();
        gameData.dataName = dataName;
        Difficulty difficulty = difficulties[difficulty];

        gameData.difficulty = difficulty;
        gameData.act = act;
        gameData.segment = segment;
        gameData.bulwarkLives = new AmountInt(difficulty.bulwarkLives);
        gameData.shrineLives = new AmountInt(difficulty.shrineLives);
        return gameData;
    }
}