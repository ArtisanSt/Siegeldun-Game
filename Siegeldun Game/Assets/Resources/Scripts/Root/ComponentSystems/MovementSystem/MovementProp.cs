using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct MovementProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [SerializeField] public float runSpeed;
    [SerializeField] public float jumpForce;
    public const float crouchMultiplier = 2 / 3;
    public Vector2 mvSpeed { get { return new Vector2(runSpeed * crouchMultiplier, runSpeed); } }

    [System.Serializable]
    public struct PassiveAbilities
    {
        public bool doubleJump;
        public bool fly;
        public float flyTime;

        public List<string> Get
        {
            get
            {
                return (from field in typeof(PassiveAbilities).GetFields()
                        where field.FieldType == typeof(bool)
                        select field.Name).ToList();
            }
        }
    }
    [SerializeField] public PassiveAbilities passiveAbilities;
}
