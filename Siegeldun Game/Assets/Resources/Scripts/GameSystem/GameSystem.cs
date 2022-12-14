using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : SingletonDontDestroy<GameSystem>
{
    //UnityEvent instanceConfigEvent;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        /*if (instanceConfigEvent == null) { instanceConfigEvent = new UnityEvent(); }
        instanceConfigEvent.AddListener(InstanceConfiguration);
        instanceConfigEvent.Invoke();*/


        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ISingleton>() != null)
                transform.GetChild(i).GetComponent<ISingleton>().InstanceConfiguration();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
