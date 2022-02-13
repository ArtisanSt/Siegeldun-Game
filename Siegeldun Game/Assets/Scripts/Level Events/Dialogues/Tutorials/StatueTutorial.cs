using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueTutorial : DialogueSystem
{
    [SerializeField] public GameObject player;

    [SerializeField] public Dialogue statueDialogue;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Player");
    }

    private bool StatueDialogueSetter(GameObject entity)
    {
        if (entity != player || statueDialogue.started) return false;

        return true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        PlayDialogue(ref statueDialogue, StatueDialogueSetter(col.gameObject));
    }
}
