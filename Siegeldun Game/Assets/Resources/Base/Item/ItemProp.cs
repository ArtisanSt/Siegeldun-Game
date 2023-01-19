using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemProp : IDataProp
{
    // ============================== MAIN PROPERTIES ==============================
    public string name;
    public string title;
    public string ID;
    public AmountInt amount;

    public enum ItemType { Null, Weapon, Gear, Key }
    public ItemType itemType;

    public string objectName => $"{itemType}_{name}";
    public string dirPath => $"Entity/{itemType}/{objectName}";
}
