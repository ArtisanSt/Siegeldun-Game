using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct BattleProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public class PassiveAbilities
    {

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
