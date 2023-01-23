using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemInitializer : MonoBehaviour
{
    public void Awake()
    {
        GameSystemInit();
        DestroyComponent();
    }

    /*[System.Serializable] public class GameOpenEvent : UnityEvent { }
    public GameOpenEvent onGameOpen;*/
    public GameObject gameSystemG;

    public void GameSystemInit() => gameSystemG.InstantiatePrefab();
    public void DestroyComponent() => Object.Destroy(this);
}
