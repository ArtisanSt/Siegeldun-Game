using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePlate : Structures, ISender
{
    [Header("STONE PLATE SETTINGS", order = 3)]
    [SerializeField] protected List<Sprite> sprites;
    [SerializeField] public bool reversible;
    [SerializeField] public bool isTimed;
    [SerializeField] public float timer;
    private float onStart = 0f;
    private float curState = 0;

    [SerializeField] protected List<GameObject> targets;

    protected override void Awake()
    {
        base.Awake();
        isInteractible = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<IActivator>() == null) return;
        if (curState == 0)
        {
            Activate();
            StartCoroutine(PlateTimer());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<IActivator>() == null || isTimed) return;
        if (curState == 1)
        {
            Deactivate();
        }
    }

    IEnumerator PlateTimer()
    {
        yield return new WaitForSeconds(timer);
        if (TimerIncrement(onStart, timer))
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        if (curState == 1) return;
        SetSprite(1);
    }

    public void Deactivate()
    {
        if (!reversible || curState == 0) return;
        SetSprite(0);
    }


    private void SetSprite(int curState)
    {
        if (curState == this.curState) return;
        this.curState = curState;
        GetComponent<SpriteRenderer>().sprite = sprites[curState];

        foreach (GameObject target in targets)
        {
            if (target.GetComponent<IReceiver>() == null) continue;

            if (curState == 0) target.GetComponent<IReceiver>().Deactivate(gameObject);
            else if (curState == 1) target.GetComponent<IReceiver>().Activate(gameObject);
        }
    }
}
