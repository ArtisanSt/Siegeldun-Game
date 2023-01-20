using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatusProp : IDataProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public struct Stats
    {
        /* Vector2
         * For Base Stats : (x : amount, y : 0f)
         * For other stats : (x : amount, y : Percent) */

        // ------------------------------ HP (Health Points) ------------------------------
        public ValueFloat HP;
        public ValueFloat HPRegen;

        // ------------------------------ SP (Stamina Points) ------------------------------
        public ValueFloat SP;
        public ValueFloat SPRegen;

        // ------------------------------ DR (Damage Reduction) ------------------------------
        public ValueFloat DR;

        // ------------------------------ Shield ------------------------------
        public ValueFloat shd;
    }

    public List<Stats> stats;
    public ValueFloat regenTimeIncrement; // HP, SP
}
