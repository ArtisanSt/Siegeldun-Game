using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct MovementProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public float runSpeed;
    public float jumpForce;
    public const float crouchMultiplier = 2 / 3;
    public Vector2 mvSpeed { get { return new Vector2(runSpeed * crouchMultiplier, runSpeed); } }

    [System.Serializable]
    public class PassiveAbilities
    {
        public Effects doubleJump;
        public Effects fly;

        public List<Effects> Get
        {
            get
            {
                return (from field in typeof(PassiveAbilities).GetFields()
                        where (bool)typeof(Effects).GetField("allow").GetValue(field.GetValue(this))
                        select (Effects)field.GetValue(this)).ToList();
            }
        }

    }


    public PassiveAbilities passiveAbilities;
}
