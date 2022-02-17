using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelProperties: MonoBehaviour
{
    [SerializeField] public static Transform resPlatform;
    [SerializeField] protected static GameObject playerPrefab;
    [SerializeField] public int actNumber;
    [SerializeField] public int lvlNumber;

    void Awake()
    {
        LevelInitialize();
        Debug.Log($"GlobalVariableStorage.difficulty {GlobalVariableStorage.curDifficulty.difficulty}");
    }

    public void LevelInitialize()
    {
        GlobalVariableStorage.curAct = actNumber;
        GlobalVariableStorage.curLvl = lvlNumber;
        ActInitialize();
        GlobalVariableStorage.curBulwarkLives = (GlobalVariableStorage.maxBulwarkLives < 0) ? 0 : GlobalVariableStorage.maxBulwarkLives;

        if (resPlatform == null) resPlatform = GameObject.Find("ResurrectionPlatform").transform;
        playerPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/EntityPrefabs/Player.prefab");
    }

    public void ActInitialize()
    {
        if (GlobalVariableStorage.curLvl != 1) return;
        GlobalVariableStorage.curEShrineLives = GlobalVariableStorage.maxEShrineLives;
    }

    public void Resurrect()
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3(resPlatform.position.x, resPlatform.position.y + 2, 0), Quaternion.identity);
    }
}