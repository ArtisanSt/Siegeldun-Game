using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{

    private const float tTimer_max = .2f;
    private int tick;
    public int tick1;
    private float tTimer;

    private void Awake()
    {
        tick = 0;
    }

    private void Update()
    {
        tTimer += Time.deltaTime;
        if(tTimer >= tTimer_max)
        {
            tTimer -= tTimer_max;
            tick++;
            tick1 = tick;
        }
    }
}
