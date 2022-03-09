using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelProperties: Singleton<LevelProperties>
{
    [SerializeField] public static Transform resPlatform;
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] public int actNumber;
    [SerializeField] public int lvlNumber;
    [SerializeField] public AudioClip lvlBGM;

    protected override void Awake()
    {
        base.Awake();
        LevelInitialize();
    }

    public void LevelInitialize()
    {
        GlobalVariableStorage.curAct = actNumber;
        GlobalVariableStorage.curLvl = lvlNumber;
        ActInitialize();
        GlobalVariableStorage.curBulwarkLives = (GlobalVariableStorage.maxBulwarkLives < 0) ? 0 : GlobalVariableStorage.maxBulwarkLives;
        SaveAndLoadManager.SaveLevelData();

        if (resPlatform == null) resPlatform = GameObject.Find("ResurrectionPlatform").transform;

        GetComponent<DialogueSystem>().EndDialogue();
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBGM(lvlBGM);
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