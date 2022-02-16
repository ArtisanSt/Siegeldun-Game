using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperties: MonoBehaviour
{
    [SerializeField] public Transform resPlatform;
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] public int actNumber;
    [SerializeField] public int lvlNumber;

    void Awake()
    {
        LevelSetup();
    }

    public void LevelSetup()
    {
        GameMechanics.gameState = GameMechanics.GameState.InGame;
        if (resPlatform == null) resPlatform = GameObject.Find("ResurrectionPlatform").transform;
    }

    public void Resurrect()
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3(resPlatform.position.x, resPlatform.position.y + 2, 0), Quaternion.identity);
    }
}