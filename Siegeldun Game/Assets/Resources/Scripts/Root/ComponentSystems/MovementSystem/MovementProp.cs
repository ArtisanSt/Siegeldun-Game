using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [SerializeField] public float runSpeed = 0;
    [SerializeField] public float jumpForce = 0;
    public const float crouchMultiplier = 2 / 3;
    public Vector2 mvSpeed { get { return new Vector2(runSpeed * crouchMultiplier, runSpeed); } }

    [SerializeField] public LayerMask groundLayer;

    [System.Serializable]
    public struct PassiveAbilities
    {
        public bool doubleJump;
        public bool canCrouch;
    }
    [SerializeField] public PassiveAbilities passiveAbilities;
}
