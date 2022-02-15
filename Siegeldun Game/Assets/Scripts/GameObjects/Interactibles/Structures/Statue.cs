using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : Structures
{
    protected override void Awake()
    {
        base.Awake();
        isInteractible = false;
    }

    protected override void Update()
    {
        base.Update();
    }

}
