using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // ========================================= Item Properties =========================================
    public string itemName;
    protected int BagQuantity;
    protected int MaxQuantity;
    protected bool hasEffect;
    protected Dictionary<string, float[]> effectDict = new Dictionary<string, float[]>()
    {
        ["HP"] = new float[2] { 0f, 0f}, // Effect Parameter, Timer
        ["Stamina"] = new float[2] { 0f, 0f },
        ["Damage"] = new float[2] { 0f, 0f },
        ["CritHit"] = new float[2] { 0f, 0f },
        ["CritChance"] = new float[2] { 0f, 0f },
        ["MVSpeed"] = new float[2] { 0f, 0f },
        ["JumpHeight"] = new float[2] { 0f, 0f },
        ["AttackSpeed"] = new float[2] { 0f, 0f },
    };
    protected float[] EffectTimers = new float[8] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };

    // ========================================= James's Properties =========================================
    public GameObject itemPrefab;
    public string itemType;
    public int amount = 0;

    public List<string> itemTypes = new List<string>()
    {
        "Consumable",
        "Weapon"
    };
}
