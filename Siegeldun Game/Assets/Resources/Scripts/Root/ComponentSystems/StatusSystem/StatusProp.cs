using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatusProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public struct Stats
    {
        /* Vector2
         * For Base Stats : (x : amount, y : 0f)
         * For other stats : (x : amount, y : Percent) */

        // ------------------------------ HP (Health Points) ------------------------------
        public Vector2 HP;
        public Vector2 HPRegen;

        // ------------------------------ SP (Stamina Points) ------------------------------
        public Vector2 SP;
        public Vector2 SPRegen;

        // ------------------------------ DR (Damage Reduction) ------------------------------
        public Vector2 DR;

        // ------------------------------ Shield ------------------------------
        public Vector2 shd;
    }

    public List<Stats> stats;
    public Vector2 regenTimeIncrement; // HP, SP
}
