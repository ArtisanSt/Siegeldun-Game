using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementProp : IDataProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [System.Serializable]
    public struct Stats
    {
        /* Vector2
         * For Base Stats : (x : amount, y : 0f)
         * For other stats : (x : amount, y : Percent) */

        public Vector2 runSpeed; // Raw Run Speed
        public Vector2 jumpForce; // Raw Jump Force
        public int jumpCount;
    }
    public bool allowed;

    public List<Stats> stats;

    public float crouchMultiplier;
    public LayerMask jumpableLayers;
}
