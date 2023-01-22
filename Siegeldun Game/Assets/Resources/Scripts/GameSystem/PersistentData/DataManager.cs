using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManager
{
    // ======================================== GameData ========================================
    public static GameData CreateNewGame(Difficulty difficulty)
    {
        GameData gameData = new GameData();
        gameData.dataName = "New Game";
        gameData.difficulty = difficulty;
        gameData.act = 0;
        gameData.segment = 0;
        gameData.bulwarkLives = gameData.difficulty.bulwarkLives;
        gameData.shrineLives = gameData.difficulty.shrineLives;
        return gameData;
    }


    // ======================================== SiegeldunData ========================================
    public static SiegeldunData LoadSiegeldunData()
    {
        string path = GameSystem.siegeldunDataPath;
        return LoadSgldnFile<SiegeldunData>(path);
    }

    public static void SaveSiegeldunData(SiegeldunData sgldn)
    {
        string path = GameSystem.siegeldunDataPath;
        CreateSgldnFile<SiegeldunData>(path, sgldn);
    }


    // ======================================== .sgldn ========================================
    public static void CreateSgldnFile<T>(string path, T data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static T LoadSgldnFile<T>(string path)
        where T : struct
    {
        if (!File.Exists(path)) CreateSgldnFile<T>(path, new T());

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        T data = (T)formatter.Deserialize(stream);
        stream.Close();
        return data;
    }

    public static void DeleteSgldnFile(string path)
    {
        if (!File.Exists(path)) return;
        File.Delete(path);
    }
}