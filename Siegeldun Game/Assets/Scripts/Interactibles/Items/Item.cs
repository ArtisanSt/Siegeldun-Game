using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelfEffectProperties
{
    // ========================================= Consumable Properties =========================================
    public string effectName { get; protected set; } // Food or Potion

    public bool hasEffect { get; protected set; }
    public float effectParam { get; protected set; }
    public string effectSpeed { get; protected set; }
    public float effectTimer { get; protected set; }

    public SelfEffectProperties(string effectName, bool hasEffect = false, float effectParam = 0f, string effectSpeed = "instant", float effectTimer = 0f)
    {
        this.effectName = effectName;
        SetValues(hasEffect, effectParam, effectSpeed, effectTimer);
    }

    public void SetValues(bool hasEffect, float effectParam, string effectSpeed, float effectTimer)
    {
        this.hasEffect = hasEffect;
        this.effectParam = effectParam;
        this.effectSpeed = effectSpeed;
        this.effectTimer = effectTimer;
    }
}

public class Item : Interactibles
{
    // ========================================= Item Properties =========================================

    public string objectName { get; protected set; }
    public string itemName { get; protected set; }
    public string itemType { get; protected set; } // Weapon, Consumable

    public int maxQuantity { get; protected set; }
    public int curQuantity { get; protected set; }

    public bool isFull { get; protected set; }
    public bool isEmpty { get; protected set; }

    public GameObject itemPrefab { get; protected set; }
    public GameObject iconPrefab { get; protected set; }

    /* Self Additional Effect
     * For timed and untimed effects
     * 
     * For Passive Effects of weapons and Consumables
     * For Active Effects of Consumables
     */

    public Dictionary<string, SelfEffectProperties> effectDict = new Dictionary<string, SelfEffectProperties>()
    {
        ["HP"] = new SelfEffectProperties("HP"),
        ["Stamina"] = new SelfEffectProperties("Stamina"),
        ["Damage"] = new SelfEffectProperties("Damage"),
        ["CritHit"] = new SelfEffectProperties("CritHit"),
        ["CritChance"] = new SelfEffectProperties("CritChance"),
        ["MVSpeed"] = new SelfEffectProperties("MVSpeed"),
        ["JumpHeight"] = new SelfEffectProperties("JumpHeight"),
        ["AttackSpeed"] = new SelfEffectProperties("AttackSpeed"),
    };


    protected void InteractInit()
    {
        isItem = true;
        isIcon = transform.parent.name != "Drops";
        canInteract = !isIcon;
        isSelected = false;

        isFull = false;
        isEmpty = false;
    }

    public bool ChangeAmount(int changeAmount)
    {
        bool processSuccessful = false;
        if (curQuantity + changeAmount >= 0 && curQuantity + changeAmount <= maxQuantity)
        {
            curQuantity += changeAmount;
            processSuccessful = true;

            if (curQuantity == 0)
            {
                isEmpty = true;
            }
            else if (curQuantity == maxQuantity)
            {
                isFull = true;
            }
            else
            {
                isFull = false;
            }
        }

        return processSuccessful;
    }

    public void PrefabsInit()
    {
        // GameObject Name Change
        gameObject.name = $"{itemName} ({gameObject.GetInstanceID()})";

        // Prefab Setting
        itemPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/ItemPrefabs/{objectName}.prefab");
        iconPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/ItemPrefabs/{objectName}_icon.prefab");
    }
}
