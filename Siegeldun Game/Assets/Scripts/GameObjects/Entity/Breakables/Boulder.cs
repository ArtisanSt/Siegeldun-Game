using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : Breakables
{
    [Header("BOULDER SETTINGS", order = 1)]
    private float circumference;
    [SerializeField] public float ratioToWorldScale = 1;
    [SerializeField] public float slowDownConst = .99f;

    // ========================================= ITEM DROPS INITIALIZATION =========================================
    protected override void itemDropsInit()
    {
        switch (difficulty)
        {
            case 1:
                // itemDrops.Add();
                break;
            case 2:
                // itemDrops.Add();
                break;
            case 3:
                // itemDrops.Add();
                break;
        }
    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        itemDropsInit();
        rBody.rotation = 45f;
        circumference = Mathf.PI * transform.localScale.x;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        MovementRotation();
    }

    private void MovementRotation()
    {
        float multiplier = slowDownConst;
        if (cirColl.IsTouchingLayers(LayerMask.GetMask("Ally", "Enemy", "Neutral"))) multiplier = 1;
        rBody.velocity = new Vector2(rBody.velocity.x * multiplier, rBody.velocity.y);

        rBody.rotation -= (rBody.velocity.x * ratioToWorldScale) / circumference;
    }

    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }

    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {

    }
}
