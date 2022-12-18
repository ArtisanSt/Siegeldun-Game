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
        public Effect doubleJump;
        public Effect fly;

        public List<Effect> Get
        {
            get
            {
                return (from field in typeof(PassiveAbilities).GetFields()
                        where (bool)typeof(Effect).GetField("allow").GetValue(field.GetValue(this))
                        select (Effect)field.GetValue(this)).ToList();
            }
        }

    }


    public PassiveAbilities passiveAbilities;
}
