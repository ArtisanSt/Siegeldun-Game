using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Structures, IReceiver
{
    [Header("GATE SETTINGS", order = 1)]
    [SerializeField] public bool closeOnTrigger = false;
    [SerializeField] public bool timed = false;
    [SerializeField] public float timer = 0f;
    private float timerStart = 0f;
    private GameObject source;
    [SerializeField] public bool isOpen = false;
    public enum ActivatorSide { Left, Right }
    [SerializeField] public ActivatorSide activatorSide;


    protected override void Awake()
    {
        base.Awake();
        isInteractible = false;
        GetComponent<Animator>().SetInteger("state", (isOpen ? 0 : 1));
    }

    protected override void Update()
    {
        base.Update();

        if (GetComponent<BoxCollider2D>().isTrigger != isOpen) { GetComponent<BoxCollider2D>().isTrigger = isOpen; }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!closeOnTrigger || col.gameObject.GetComponent<IActivator>() == null) return;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!closeOnTrigger || col.gameObject.GetComponent<IActivator>() == null || col.gameObject.GetComponent<Beings>() == null || (col.gameObject.transform.position.x - transform.position.x) * ((activatorSide == ActivatorSide.Left) ? 1 : -1 ) <= 0) return;
        Deactivate(source);
    }

    public void Activate(GameObject source)
    {
        if (isOpen) return;
        GetComponent<Animator>().SetInteger("state", 0);
        this.source = source;
        isOpen = true;
    }

    IEnumerator DeactivateTimer()
    {
        yield return new WaitForSeconds(timer);
        if (TimerIncrement(timerStart, timer))
        {
            Deactivate(source);
        }
    }

    public void Deactivate(GameObject source)
    {
        if (!isOpen) return;
        GetComponent<Animator>().SetInteger("state", 1);
        this.source = source;
        isOpen = false;
        source.GetComponent<ISender>().Deactivate();
    }
}
