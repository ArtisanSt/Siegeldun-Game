using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityContainer : MonoBehaviour
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    // When turned disabled
    protected virtual void OnDisable()
    {

    }

    // When turned enabled
    protected virtual void OnEnable()
    {

    }

    // When scene ends
    protected virtual void OnDestroy()
    {

    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    [SerializeField] public static string ScriptableObjectsPath = "Assets/Resources/Scriptable Objects";
    [SerializeField] public int entityInstanceCap;
    public bool ignoreCap; // Debugging

    public interface IRowProperty { }

    [System.Serializable]
    public abstract class RowProperty<N, T>
        where N: EntityProp
        where T: Entity<N>
    {
        public EntityProp.EntityType entityType
        {
            get
            {
                System.Enum.TryParse(typeof(T).Name, out EntityProp.EntityType entityType);
                return entityType;
            }
        }
        public EntityProp.EntitySubType entitySubType
        {
            get
            {
                System.Enum.TryParse(typeof(T).Name, out EntityProp.EntitySubType entitySubType);
                return entitySubType;
            }
        }
        public string entityName;
        public bool allow;
        public int instanceCap;
    }


    [SerializeField] public List<RowProperty<UnitProp, Unit<UnitProp>>> unitList;
    [SerializeField] public List<RowProperty<ItemProp, Item<ItemProp>>> itemList;
    [SerializeField] public List<RowProperty<StructureProp, Structure<StructureProp>>> structureList;

    public static string[] IdentifyType(string entityName)
    {
        foreach (List<RowProperty<EntityProp, Entity<EntityProp>>> rp in new List<List<RowProperty<EntityProp, Entity<EntityProp>>>>())
        {
            for (int i = 0; i < rp.Count; i++)
            {
                if (entityName.Trim() == rp[i].entityName.Trim())
                {
                    return new string[2] { rp[i].entityType.ToString() , rp[i].entitySubType.ToString() };
                }
            }
        }

        return new string[0];
    }
}
