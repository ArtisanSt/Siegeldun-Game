using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structures : Interactibles
{
    // ========================================= Structure Properties =========================================
    public abstract string structureName { get; }


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
}
