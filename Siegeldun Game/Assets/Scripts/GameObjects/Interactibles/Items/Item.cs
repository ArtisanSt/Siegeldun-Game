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
    public string effectSpeed { get; protected set; } // "Instant": (HP and Stamina: Affects insantly, Others: Until weapon is unequipped), "Overtime"
    public float effectTimer { get; protected set; }
    public float effectTimerIncrements { get; protected set; }

    public SelfEffectProperties(string effectName, bool hasEffect = false, float effectParam = 0f, string effectSpeed = "instant", float effectTimer = 0f, float effectTimerIncrements = .1f)
    {
        this.effectName = effectName;
        SetValues(hasEffect, effectParam, effectSpeed, effectTimer, effectTimerIncrements);
    }

    public void SetValues(bool hasEffect, float effectParam, string effectSpeed, float effectTimer, float effectTimerIncrements)
    {
        this.hasEffect = hasEffect;
        this.effectParam = effectParam;
        this.effectSpeed = effectSpeed;
        this.effectTimerIncrements = effectTimerIncrements;
    }
}

public abstract class Item : Interactibles, IInteractor
{
    // ========================================= Item Properties =========================================
    public abstract string itemName { get; }
    public abstract string itemType { get; } // Weapon, Consumable, Key

    [Header("ITEM SETTINGS", order = 1)]
    [SerializeField] private int _maxQuantity = 1;
    public int maxQuantity { get { return _maxQuantity; } protected set { _maxQuantity = value; } }
    [SerializeField] private int _curQuantity = 1;
    public int curQuantity { get { return _curQuantity; } protected set { _curQuantity = value; } }
    public int amountOverflow { get; protected set; }

    public bool isFull { get; protected set; }
    public bool isEmpty { get; protected set; }

    [SerializeField] private GameObject _itemPrefab = null;
    public GameObject itemPrefab { get { return _itemPrefab; } protected set { _itemPrefab = value; } }
    [SerializeField] private GameObject _iconPrefab = null;
    public GameObject iconPrefab { get { return _iconPrefab; } protected set { _iconPrefab = value; } }

    [SerializeField] public bool equippable = false;
    private bool _inInventory = false;
    public bool inInventory { get { return _inInventory; } protected set { _inInventory = value; } }
    private bool _isEquipped = false;
    public bool isEquipped { get { return _isEquipped; } protected set { _isEquipped = value; } }

    public GameObject entityHolder { get; protected set; }

    private bool _hasReference = false;
    public bool hasReference { get { return _hasReference; } protected set { _hasReference = value; } }
    public GameObject referencedItem { get; protected set; }

    public bool doMerge { get; protected set; } // A condition if the item is allowed to merge with other colliding same items
    public bool canMerge { get; protected set; } // Updates every frame

    [Header("ITEM MOVEMENT SETTINGS", order = 2)]
    [SerializeField] protected Rigidbody2D rBody;

    [Header("ITEM BOUNCE SETTINGS", order = 3)]
    [SerializeField] private float itemHopSpeed = 0.0075f;
    [SerializeField] private float hopLimit = 0.05f;
    private int hopDirection = 1;
    private Vector2 itemOrigPosition;

    [Header("ITEM MERGE SETTINGS", order = 4)]
    [SerializeField] protected Interactor interactor;
    [SerializeField] private float itemPullSpeed = 0.0075f;
    private int pullDirection = 0;
    protected GameObject nearestItem;
    private bool isMerging = false;
    public enum MergeState { Lead, Other}

    /* Self Additional Effect
     * For timed and untimed effects
     * 
     * For Passive Effects of weapons and Consumables
     * For Active Effects of Consumables
     */

    protected void ItemInit()
    {
        bool isIcon = transform.parent.name != "Drops";
        isInteractible = !isIcon;

        objectClassification = (isIcon) ? "ICON" : "ITEM";
        if (itemType == "Weapon" || itemType == "Consumable") { equippable = true; }

        isFull = false;
        isEmpty = false;

        amountOverflow = 0;
        
        if (objectClassification == "ITEM")
        {
            rBody = GetComponent<Rigidbody2D>();
            itemOrigPosition = rBody.position;

            interactor = GetComponent<Interactor>();
        }
    }

    protected virtual void Awake()
    {
        GameMechanicsPropInit();
        ItemInit();
        UniqueStatsInit();
    }

    protected virtual void Update()
    {
        isFull = curQuantity >= maxQuantity;
        isEmpty = curQuantity <= 0;

        ItemUpdate();
        IconUpdate();

    }

    private void IconUpdate()
    {
        if (objectClassification != "ICON") return;

        if (hasReference)
        {
            ReferencedItemUpdate();
        }
        else if (inInventory)
        {
            if (isEquipped) PassiveEffects("Equipped");
            else PassiveEffects("Inventory");
        }
    }

    private void ItemUpdate()
    {
        if (objectClassification != "ITEM") return;

        if (isEmpty) Destroy(gameObject);

        if ((rBody.position.y - itemOrigPosition.y) * hopDirection >= hopLimit) { hopDirection *= -1; }
    }

    void FixedUpdate()
    {
        if (objectClassification != "ITEM") return;

        rBody.MovePosition(new Vector2(ItemPull(), ItemBounce()));

        if (interactor == null || isMerging || interactor.curSelected == null) return;
        Merge(nearestItem);
    }

    private float ItemBounce()
    {
        return rBody.position.y + itemHopSpeed * hopDirection;
    }

    private float ItemPull()
    {
        if (interactor == null || isMerging || interactor.curSelected == null) return rBody.position.x;

        nearestItem = interactor.curSelected;
        pullDirection = (nearestItem.transform.position.x - transform.position.x > 0) ? 1 : -1 ;
        return rBody.position.x + itemPullSpeed * pullDirection * (1 + Time.fixedDeltaTime);

    }

    public void Merge(GameObject otherObject)
    {
        if (Mathf.Abs(otherObject.transform.position.x - transform.position.x) > 0.1f) return;

        Item otherItem = otherObject.GetComponent<Item>();
        if (otherItem.itemName != itemName || otherItem.isFull) return;

        MergeState state = (gameObject.GetInstanceID() < otherObject.GetInstanceID()) ? MergeState.Lead : MergeState.Other;
        isMerging = true;
        ChangeAmount(otherItem.curQuantity);
        Destroy(otherObject);
        isMerging = false;

    }

    public bool InteractorColliderConditions(Collider2D col)
    {
        return col.gameObject != gameObject && col.gameObject.GetComponent<Item>().itemName == itemName;
    }

    protected abstract void UniqueStatsInit();

    public Dictionary<string, SelfEffectProperties> effectDict = new Dictionary<string, SelfEffectProperties>()
    {
        ["HP"] = new SelfEffectProperties("HP"),
        ["Stamina"] = new SelfEffectProperties("Stamina"),

        ["Damage"] = new SelfEffectProperties("Damage"),
        ["AttackRange"] = new SelfEffectProperties("AttackRange"),
        ["AttackSpeed"] = new SelfEffectProperties("AttackSpeed"),
        ["AttackDelay"] = new SelfEffectProperties("AttackDelay"),

        ["CritHit"] = new SelfEffectProperties("CritHit"),
        ["CritChance"] = new SelfEffectProperties("CritChance"),

        ["WpnStamCost"] = new SelfEffectProperties("WpnStamCost"),

        ["KbForce"] = new SelfEffectProperties("KbForce"),

        ["MVSpeed"] = new SelfEffectProperties("MVSpeed"),
        ["JumpHeight"] = new SelfEffectProperties("JumpHeight"),

        ["KbReduction"] = new SelfEffectProperties("KbReduction"),
        ["DmgReduction"] = new SelfEffectProperties("DmgReduction"),
        ["CritReduction"] = new SelfEffectProperties("CritReduction"),
    };

    public int ChangeAmount(int changeAmount) // -1: Fail, 0: Success, 1: Overflow
    {
        Debug.Log(gameObject);

        if (changeAmount == 0) return -1;

        int output = -1;
        int newAmount = curQuantity + changeAmount;
        isFull = false; // Temporary Change
        
        // Amount Decreased
        if (newAmount < curQuantity)
        {
            if (newAmount >= 0)
            {
                output = 0;
                curQuantity = newAmount;
            }
            isEmpty = curQuantity == 0;
        }

        // Amount Increased
        else if (newAmount > curQuantity)
        {
            output = 0;
            if (newAmount > maxQuantity)
            {
                output = 1;
                amountOverflow = curQuantity - maxQuantity;
                curQuantity = maxQuantity;

                GameObject newDrop = Drop(1, new Vector2(((Random.Range(0, 2) == 0) ? -5 : 5 ), 0), itemPrefab);

                newDrop.GetComponent<Item>().OverwriteStats(curQuantity, amountOverflow);
            }
            else
            {
                curQuantity = newAmount;
            }
            isFull = curQuantity == maxQuantity;
        }

        return output;
    }

    protected abstract void PassiveEffects(string state);

    protected abstract void ActiveEffects();

    public virtual bool ActiveEffectsCondition()
    {
        return ChangeAmount(-1) >= 0;
    }

    protected void UseEffects()
    {
        foreach (KeyValuePair<string, SelfEffectProperties> eachEffect in effectDict)
        {
            UseEffects(eachEffect.Key);
        }
    }

    protected void UseEffects(string effectName)
    {
        if (effectDict[effectName].hasEffect)
        {
            if ((effectName == "HP" || effectName == "Stamina") && entityHolder.GetComponent<IRegeneration>() != null) entityHolder.GetComponent<IRegeneration>().Regenerate(effectName, effectDict[effectName].effectParam, effectDict[effectName].effectSpeed, effectDict[effectName].effectTimer, effectDict[effectName].effectTimerIncrements);
            else
            {
                entityHolder.GetComponent<IBoostable>().AddBoost(effectName, gameObject.name, effectDict[effectName]);
            }
        }
    }

    public virtual void OnSelect(GameObject entityHolder)
    {
        this.entityHolder = entityHolder;
        PassiveEffects("Select");
    }

    public virtual void OnInventory(GameObject entityHolder, GameObject inventorySlot)
    {
        inInventory = true;
        this.entityHolder = entityHolder;
        PassiveEffects("Inventory");
    }

    public virtual void OnEquip(GameObject entityHolder)
    {
        isEquipped = true;
        this.entityHolder = entityHolder;
        PassiveEffects("Equipped");
    }

    public virtual void OnUnequip()
    {
        isEquipped = false;
    }

    public virtual bool OnUse(bool isCrit)
    {
        bool outcome = ActiveEffectsCondition();
        if (outcome) ActiveEffects();
        return outcome;
    }

    protected override void PrefabsInit()
    {
        // Prefab Setting
        itemPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/ItemPrefabs/{objectName}.prefab");
        iconPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/ItemPrefabs/{objectName}_icon.prefab");

        // GameObject Name Change
        gameObject.name = $"{itemName} ({gameObject.GetInstanceID()})";
    }

    public void OverwriteStats(int curQuantity, int amountOverflow)
    {
        this.curQuantity = curQuantity;
        this.amountOverflow = amountOverflow;
    }

    public void ItemReferencedTo(GameObject referencedItem)
    {
        hasReference = referencedItem != null;
        if (hasReference) this.referencedItem = referencedItem;
    }

    private void ReferencedItemUpdate()
    {
        int curQuantity = referencedItem.GetComponent<Item>().curQuantity;
        if (this.curQuantity != curQuantity) { this.curQuantity = curQuantity; }
    }
}
