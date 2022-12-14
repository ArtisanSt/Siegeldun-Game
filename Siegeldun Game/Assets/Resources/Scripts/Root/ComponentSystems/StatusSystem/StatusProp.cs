using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public struct PassiveAbilities
    {
        public bool hpRegen;
        public bool spRegen;
    }
    public PassiveAbilities passiveAbilities;

}
