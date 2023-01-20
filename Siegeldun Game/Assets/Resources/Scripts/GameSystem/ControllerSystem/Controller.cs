using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    /*  COMPONENT FOR ENTITIES */

    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        ControllerInit();
    }

    protected virtual void Update()
    {
        ControllerTypeIdentifier();
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    // When turned disabled
    protected virtual void OnDisable()
    {

    }

    // When turned enabled
    protected virtual void OnEnable()
    {

    }

    // When scene ends
    protected virtual void OnDestroy()
    {

    }


    // ============================== COMPONENTS ==============================
    protected IControllable[] iControllables;

    public class Controls
    {
        // ------------------------------- Movement -------------------------------
        public float horizontal;
        public float vertical;
        public bool jump;
        public bool fly;
        public bool dash;
        public bool crouch;

        // ------------------------------- Battle -------------------------------
        public bool attack;
        public bool block;
        public bool ability;
        public bool reload;

        // ------------------------------- Interact -------------------------------
        public bool interact;
        public bool consume;
        public bool inventory;
        public bool map;

    }

    public Controls controls { get; private set; }

    protected virtual void ControllerInit()
    {
        iControllables = GetComponents<IControllable>();
        controls = new Controls();
    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    public enum ControllerType { None, AI, Player }
    public ControllerType controllerType;

    public static ControllerType None => ControllerType.None;
    public static ControllerType Player => ControllerType.Player;
    public static ControllerType AI => ControllerType.AI;

    public void SetControllerType(ControllerType controllerType)
    {
        this.controllerType = controllerType;
    }

    private bool crouch = false;


    public void ControllerTypeIdentifier()
    {
        if (iControllables.Length <= 0) return;

        switch (controllerType)
        {
            case ControllerType.Player:
                ControllerPlayer();
                break;

            case ControllerType.AI:
                ControllerAI();
                break;

            default:
                break;
        }

        for (int i=0; i<iControllables.Length; i++)
        {
            iControllables[i].Receiver(controls);
        }
    }

    public void ControllerPlayer()
    {
        PlayerBattle();
        PlayerInteract();
        PlayerMovement();
    }

    public void PlayerMovement()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (ControllerSystem.instance.crouchOn == ControllerSystem.CrouchOn.Hold)
                crouch = Input.GetButtonDown("Crouch");
            else
                crouch = !crouch;
        }

        controls.horizontal = Input.GetAxisRaw("Horizontal");
        controls.jump = Input.GetButtonDown("Jump");
        controls.vertical = Input.GetAxisRaw("Vertical");
        controls.dash = Input.GetButtonDown("Dash");
        controls.fly = Input.GetButtonDown("Jump");
        controls.crouch = crouch;
    }

    public void PlayerBattle()
    {
        controls.attack = Input.GetButtonDown("Attack");
        controls.block = Input.GetButtonDown("Block");
        controls.ability = Input.GetButtonDown("Ability");
        controls.reload = Input.GetButtonDown("Reload");


    }

    public void PlayerInteract()
    {
        controls.interact = Input.GetButtonDown("Interact");
        controls.consume = Input.GetButtonDown("Consume");
        controls.inventory = Input.GetButtonDown("Inventory");
        controls.map = Input.GetButtonDown("Map");


    }

    public void ControllerAI()
    {

    }
}
