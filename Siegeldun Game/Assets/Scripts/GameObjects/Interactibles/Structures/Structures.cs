using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structures : Interactibles
{
    // ========================================= Structure Properties =========================================
    [Header("STRUCTURE SETTINGS", order = 1)]
    [SerializeField] public string structureName;


    protected virtual void Awake()
    {
        GameMechanicsPropInit();
        StructureInit();
    }


    protected void StructureInit()
    {
        objectClassification = "STRUCTURE";
        isInteractible = true;
    }

    protected virtual void Update()
    {
        if (!PauseMechanics.isPlaying) return;
    }
}
