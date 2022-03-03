using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobtaDeath : MonoBehaviour
{
    [SerializeField] private GameObject gobta;
    [SerializeField] private GameObject gate;
    private bool isDone = false;

    [SerializeField] private AudioClip gateOpenSfx;

    void Start()
    {
        gate.GetComponent<Animator>().SetTrigger("Close");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDone && gobta == null)
            OpenGate();
    }

    void OpenGate()
    {
        Debug.Log("Open Gate");
        isDone = true;
        SoundManager.instance.PlaySFX(gateOpenSfx);
        gate.GetComponent<Animator>().SetTrigger("Open");
        gate.GetComponent<BoxCollider2D>().enabled  = false;
    }
}
