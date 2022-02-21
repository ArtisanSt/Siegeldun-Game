using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Structures
{
    [SerializeField] public GameMechanics gameMechanics;

    protected override void Awake()
    {
        base.Awake();
        gameMechanics = GameObject.Find("Game System/GameMechanics").GetComponent<GameMechanics>();
    }

    protected override void Update()
    {
        base.Update();
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected) return;

        Debug.Log("ENTERING NEXT STAGE");
        gameMechanics.StartNextLevel();
    }
}
