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
        ComponentChecker();
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
    protected MovementSystem movementSystem;
    protected BattleSystem battleSystem;
    //protected Interactible cirCol;

    protected virtual void ComponentChecker()
    {
        movementSystem = GetComponent<MovementSystem>();
        battleSystem = GetComponent<BattleSystem>();

    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    public enum ControllerType { None, AI, Player }
    public ControllerType controllerType;

    private bool crouch = false;


    public void ControllerTypeIdentifier()
    {
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
    }

    public void ControllerPlayer()
    {
        PlayerBattle();
        PlayerInteract();
        PlayerMovement();
    }

    public void PlayerMovement()
    {
        if (movementSystem == null) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool climb = Input.GetButtonDown("Climb");
        bool dash = Input.GetButtonDown("Dash");
        bool fly = Input.GetButton("Jump");

        if (Input.GetButtonDown("Crouch"))
        {
            if (ControllerSystem.crouchOn == ControllerSystem.CrouchOn.Hold)
                crouch = Input.GetButtonDown("Crouch");
            else
                crouch = !crouch;
        }

        movementSystem.Receiver(horizontal, jump, fly, climb, dash, crouch);
    }

    public void PlayerBattle()
    {
        if (battleSystem == null) return;

        bool attack = Input.GetButtonDown("Attack");
        bool ability = Input.GetButtonDown("Ability");
        bool reload = Input.GetButtonDown("Reload");


    }

    public void PlayerInteract()
    {
        //if (battleSystem == null) return;

        bool interact = Input.GetButtonDown("Interact");
        bool consume = Input.GetButtonDown("Consume");
        bool inventory = Input.GetButtonDown("Inventory");
        bool map = Input.GetButtonDown("Map");


    }


    public void ControllerAI()
    {

    }
}
