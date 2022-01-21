using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Entity : MonoBehaviour
{
    // ========================================= Game Properties =========================================
    [SerializeField] protected static int difficulty = 1; // Pseudo Difficulty
    protected int idxDiff = difficulty - 1;
    protected float animationSpeed = 1f;


    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    public Rigidbody2D rBody;
    public SpriteRenderer sprite;
    protected BoxCollider2D boxColl;
    protected CapsuleCollider2D capColl;
    protected Animator anim;

    protected GameObject entityGameObject;
    public GameObject entityPrefab;

    protected enum MovementAnim { idle, run, jump, fall };
    protected MovementAnim state;

    protected void ComponentInitialization()
    {
        rBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();
        if (entityType == "Beings")
        {
            capColl = GetComponent<CapsuleCollider2D>();
        }
        anim = GetComponent<Animator>();
        entityGameObject = gameObject;
    }


    // ========================================= Entity Properties =========================================
    protected string entityName;
    protected string entityType; // "Beings" or "Breakables"
    [SerializeField] protected List<GameObject> entityDrops;
    protected bool doDrop;
    protected bool willBeDestroyed;
    public int entityID;
    protected int dropChance;
    protected List<int> curProcess = new List<int>();

    // Base HP Mechanics
    public bool isAlive = true;
    protected bool deadBeings;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float entityHp;

    // Base Mechanics
    protected float lastAttack;
    [SerializeField] protected float attackSpeed;
    protected bool isKnockbacked;
    protected float knockbackedForce;
    protected int kbDir; // 1 is right, -1 is left, 0 when not attacking
    protected float kTick;

    [SerializeField] protected float hpRegenTimer;

    // Entity Static Properties
    public static Dictionary<string, List<GameObject>> EntityInstances = new Dictionary<string, List<GameObject>>();


    protected void PrefabsInit()
    {
        entityPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/EntityPrefabs/{entityName}.prefab");

        // Entity Instance
        if (EntityInstances.ContainsKey(entityName))
        {
            EntityInstances[entityName].Add(gameObject);
        }
        else
        {
            EntityInstances.Add(entityName, new List<GameObject>() { gameObject });
        }
        if (entityName != "Player")
        {
            gameObject.name = $"{entityName} ({entityID})";
        }
    }


    // ========================================= ENTITY METHODS =========================================
    // Damage Receive
    public void TakeDamage(float damageTaken, int attackID, int kbDir, float knockbackedForce, bool isCrit = false)
    {
        if (isAlive && damageTaken > 0f && !curProcess.Contains(attackID))
        {
            curProcess.Add(attackID);

            hpRegenTimer = 0f;
            if (damageTaken >= Mathf.Floor(entityHp))
            {
                entityHp -= entityHp;
                Die();
            }
            else
            {
                entityHp -= damageTaken;
                if (entityType == "Beings")
                {
                    if (isCrit)
                    {
                        lastAttack = Time.time;
                        anim.SetTrigger("hurt");
                        Knockback(kbDir, knockbackedForce);
                    }
                }
            }

            StartCoroutine(ClearID(attackID, attackSpeed));
        }
    }

    public void Knockback(int kbDir, float kbHorDisplacement)
    {
        isKnockbacked = true;
        knockbackedForce = kbHorDisplacement * 5f;
        this.kbDir = kbDir;
        kTick = Time.deltaTime;
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log(entityName + " Dead!");
        anim.SetBool("death", true);
        EntityInstances[entityName].Remove(gameObject);
    }

    protected void ClearInstance(int time = 1)
    {
        if (boxColl.enabled && deadBeings && !willBeDestroyed)
        {
            if (entityType == "Beings")
            {
                capColl.enabled = false;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            boxColl.enabled = false;
            willBeDestroyed = true;

            Drop(dropChance, 0f);
            StartCoroutine(DestroyInstance(time));
        }
    }

    private IEnumerator DestroyInstance(int time = 1)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    protected IEnumerator ClearID(int instanceID, float time = 1)
    {
        yield return new WaitForSeconds(time);
        curProcess.Remove(instanceID);
    }


    // ========================================= GAMEPLAY METHODS INITIALIZATION =====================================
    public GameObject Drop(int chance, float xPos = 0, float yPos = 0, GameObject itemDrop = null, Transform parentTransform = null)
    {
        if (Random.Range(1, chance + 1) == 1 && doDrop)
        {
            if (itemDrop == null)
            {
                itemDrop = entityDrops[Random.Range(0, entityDrops.Count)];
            }

            if (parentTransform == null)
            {
                parentTransform = GameObject.Find("Drops").transform;
            }

            GameObject newDrop = (GameObject)Instantiate(itemDrop, new Vector3(transform.position.x + xPos, transform.position.y + yPos, 0), Quaternion.identity, parentTransform);
            return newDrop;
        }
        else
        {
            return null;
        }
    }
}
