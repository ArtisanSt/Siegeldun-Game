using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameDataManager
{
    public static List<GameData> savedData { get; private set; }
    public static GameData lastLoadedData { get; private set; }
    public static int savedDataCap { get; set; }

    private static int loadedDataIndex;
    public static GameData currentData { get; set; }

    public static void DataInit()
    {
        loadedDataIndex = -1;
        /* Locate GameData JSON file
         * Fetch all the data
        */
        savedData.Add(new GameData());

    }

    public static bool CreateNewData()
    {
        if (savedData.Count >= savedDataCap) return false;

        return true;
    }

    public static bool SaveData(GameData gameData)
    {
        if (gameData == null) return false;

        /* Locate JSON file
         * overrides the last data with the current data in the JSON file
         * overrides the last saved data (property) with the current data
        */
        return true;
    }

    public static bool LoadSavedData(int dataIndex)
    {
        if (!savedData.FindIndex(dataIndex)) return false;

        loadedDataIndex = dataIndex;
        LoadData(savedData[dataIndex]);
        return true;
    }

    private static void LoadData(GameData loadedData)
    {
        lastLoadedData = loadedData;
        currentData = loadedData;
    }

    public static void UpdateData()
    {
        if (!savedData.FindIndex(loadedDataIndex)) return;

        savedData[loadedDataIndex] = currentData;
        SaveData(currentData);
    }

    public static void RevertToLastSavedData()
    {
        if (!savedData.FindIndex(loadedDataIndex)) return;

        //currentData = lastLoadedData.Duplicate();
    }

    private static bool FindIndex(this IList values, int index)
    {
        return Enumerable.Range(0, values.Count).Contains(index);
    }
}

/* JSON
 * 
 * int savedDataCap
 *  JSONObject savedData
*/
