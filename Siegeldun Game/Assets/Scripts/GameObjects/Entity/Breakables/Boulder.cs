using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : Breakables, IActivator
{
    public enum Shape { Circle, Square }
    [Header("BOULDER SETTINGS", order = 1)]
    [SerializeField] public Shape shape;
    private float circumference;
    [SerializeField] public float ratioToWorldScale = 1;
    [SerializeField] public float slowDownConst = .99f;

    // ========================================= ITEM DROPS INITIALIZATION =========================================
    protected override void itemDropsInit()
    {

    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        itemDropsInit();
        if (shape == Shape.Circle)
        {
            rBody.rotation = 45f;
            circumference = Mathf.PI * transform.localScale.x;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        MovementRotation();
    }

    private void MovementRotation()
    {
        if (shape != Shape.Circle) return;

        float multiplier = slowDownConst;
        if (cirColl.IsTouchingLayers(LayerMask.GetMask("Ally", "Enemy", "Neutral"))) multiplier = 1;
        rBody.velocity = new Vector2(rBody.velocity.x * multiplier, rBody.velocity.y);

        rBody.rotation -= (rBody.velocity.x * ratioToWorldScale) / circumference;
    }

    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {

    }
}
