using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityProp : IDataProp
{
    // ============================== MAIN PROPERTIES ==============================
    public string name;
    public string title;
    public string ID;
    public int instanceCap;

    public enum EntityRace { Human, Orc }
    public EntityRace entityRace;

    public string objectName => $"{entityRace}_{name}";
    public string dirPath => $"Entity/{entityRace}/{objectName}" ;
}
