using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // ========================================= Item Properties =========================================
    public string itemName;
    public string itemType;
    public int maxQuantity;
    public GameObject itemPrefab;
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

    protected void ItemEffectInitialization()
    {
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
}
