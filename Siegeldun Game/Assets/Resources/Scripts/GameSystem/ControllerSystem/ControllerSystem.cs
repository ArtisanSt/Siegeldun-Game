using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSystem : SettingsSystem<ControllerSystem>
{
    /*  COMPONENT FOR GAME SYSTEMS */

    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

    }

    // When turned disabled
    protected override void OnDisable()
    {
        base.OnDisable();

    }

    // When turned enabled
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    // When scene ends
    protected override void OnDestroy()
    {
        base.OnDestroy();

    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    public static GameObject player { get; private set; }

    public enum CrouchOn { Hold, Toggle }
    public CrouchOn crouchOn;

    public void SetPlayer(GameObject player)
    {
        if (ControllerSystem.player != null)
            ControllerSystem.player.GetComponent<Controller>().SetControllerType(Controller.None);

        if (player == null) return;
        ControllerSystem.player = player;
        ControllerSystem.player.GetComponent<Controller>().SetControllerType(Controller.Player);
    }
}