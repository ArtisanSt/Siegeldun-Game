using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BattleProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public struct Stats
    {
        /* Vector2
         * For Base Stats : (x : amount, y : 0f)
         * For other stats : (x : amount, y : Percent) */

        // HP (Health Points)
        public Vector2 dmg; // Raw damage

        public Vector2 prgDmg; // Piercing Damage [ignores damage resistance]
        public Vector2 trDmg; // True Damage [ignores damage resistance and shield]
        public Vector2 ftlDmg; // Fatal Damage [additional damage to instantly kill an enemy]

        public Vector2 atkSpd;

        public float critHit; // Percent
        public float critCnc; // Percent
    }

    public List<Stats> stats;

}
