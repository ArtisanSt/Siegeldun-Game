using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct StatusProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public class PassiveAbilities
    {
        public Effect hpRegen;
        public Effect spRegen;

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
