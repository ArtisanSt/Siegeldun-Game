using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : Interactibles
{
    // ========================================= Item Properties =========================================
    public string itemName { get; protected set; }
    public string itemType { get; protected set; }
    public int maxQuantity { get; protected set; }
    public int curQuantity;
    public GameObject itemPrefab { get; protected set; }
    public GameObject iconPrefab { get; protected set; }
    public Dictionary<string, Dictionary<string, float>> effectDict = new Dictionary<string, Dictionary<string, float>>()
    {
        ["HP"] = new Dictionary<string, float>(),
        ["Stamina"] = new Dictionary<string, float>(), 
        ["Damage"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["CritHit"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["CritChance"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["MVSpeed"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, distance
        ["JumpHeight"] = new Dictionary<string, float>(), //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
        ["AttackSpeed"] = new Dictionary<string, float>() //0:false or 1:true, Effect Parameter, Effect Timer, number of uses
    };
    //protected float[] EffectTimers = new float[8] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };


    protected void ItemInit()
    {
        _isIcon = transform.parent.name != "Drops";
        if (_isIcon)
        {
            ComponentInt();
        }
        isItem = true;
        effectDict["HP"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
            ["effectParam"] = 0f, // How much it heals
            ["effectSpeed"] = 0f, // 0: instant, 1: overtime
            ["effectTimer"] = 0f, // Time of effect
        };

        effectDict["Stamina"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
            ["effectParam"] = 0f, // How much it heals
            ["effectSpeed"] = 0f, // 0: instant, 1: overtime
            ["effectTimer"] = 0f, // Time of effect
        };

        effectDict["Damage"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
        };

        effectDict["CritHit"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
        };

        effectDict["CritChance"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
        };

        effectDict["MVSpeed"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
        };

        effectDict["JumpHeight"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
        };

        effectDict["AttackSpeed"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
        };
    }

    public void PrefabsInit()
    {
        itemPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/ItemPrefabs/{itemName}.prefab");
        iconPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/ItemPrefabs/{itemName}_icon.prefab");
    }
}
