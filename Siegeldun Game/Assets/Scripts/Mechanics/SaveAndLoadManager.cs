using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class LevelData
{
    public DifficultyProperties curDifficulty = new DifficultyProperties();
    public int curAct = 0;
    public int curLvl = 1;


    public void SaveLevelDataToStorageData()
    {
        GlobalVariableStorage.curDifficulty = curDifficulty;
        GlobalVariableStorage.curAct = curAct;
        GlobalVariableStorage.curLvl = curLvl;
    }

    public void SaveStorageDataToLevelData()
    {
        curDifficulty = GlobalVariableStorage.curDifficulty;
        curAct = GlobalVariableStorage.curAct;
        curLvl = GlobalVariableStorage.curLvl;
    }
}

[System.Serializable]
public class GameData
{
    // Achievements
    public bool[] achievements = new bool[4] { false, false, false, false };


    // Unlocks
    public bool[] hostiles = new bool[4] { false, false, false, false };
    public bool[] potions = new bool[2] { false, false };
    public bool[] weapons = new bool[1] { false };

    public void SaveGameDataToStorageData()
    {
        GlobalVariableStorage.achievements = achievements;
        GlobalVariableStorage.hostiles = hostiles;
        GlobalVariableStorage.potions = potions;
        GlobalVariableStorage.weapons = weapons;
    }

    public void SaveStorageDataToGameData()
    {
        achievements = GlobalVariableStorage.achievements;
        hostiles = GlobalVariableStorage.hostiles;
        potions = GlobalVariableStorage.potions;
        weapons = GlobalVariableStorage.weapons;
    }
}

public static class SaveAndLoadManager
{
    public static void SaveLevelData()
    {

    }

    public static void LoadLevelData()
    {

    }

    public static void SaveGameData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData.ats";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData();
        gameData.SaveStorageDataToGameData();

        formatter.Serialize(stream, gameData);
        //stream.Seek(0, SeekOrigin.Begin);
        stream.Close();
    }

    public static void LoadGameData()
    {
        string path = Application.persistentDataPath + "/GameData.ats";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //stream.Seek(0, SeekOrigin.Begin);
            GameData gameData = (GameData)formatter.Deserialize(stream);

            gameData.SaveGameDataToStorageData();

            stream.Close();
        }
        else
        {
            Debug.LogError($"Save File not found in {path}!");
            SaveGameData();
        }
    }
}
