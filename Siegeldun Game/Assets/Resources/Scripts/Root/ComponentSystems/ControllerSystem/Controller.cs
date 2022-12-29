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
    protected MovementSystem movementSystem;
    protected BattleSystem battleSystem;
    protected IControllable iControllable;
    //protected Interactible cirCol;

    private Dictionary<string, object> controls;

    protected virtual void ControllerInit()
    {
        iControllable = GetComponent<IControllable>();
        controls = new Dictionary<string, object>();
    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    public enum ControllerType { None, AI, Player }
    public ControllerType controllerType;

    private bool crouch = false;


    public void ControllerTypeIdentifier()
    {
        if (iControllable == null) return;

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

        iControllable.Receiver(controls);
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

        controls["horizontal"] = Input.GetAxisRaw("Horizontal");
        controls["jump"] = Input.GetButtonDown("Jump");
        controls["vertical"] = Input.GetAxisRaw("Vertical");
        controls["dash"] = Input.GetButtonDown("Dash");
        controls["fly"] = Input.GetButtonDown("Jump");
        controls["crouch"] = crouch;
    }

    public void PlayerBattle()
    {
        bool attack = Input.GetButtonDown("Attack");
        bool ability = Input.GetButtonDown("Ability");
        bool reload = Input.GetButtonDown("Reload");


    }

    public void PlayerInteract()
    {
        bool interact = Input.GetButtonDown("Interact");
        bool consume = Input.GetButtonDown("Consume");
        bool inventory = Input.GetButtonDown("Inventory");
        bool map = Input.GetButtonDown("Map");


    }


    public void ControllerAI()
    {

    }
}
