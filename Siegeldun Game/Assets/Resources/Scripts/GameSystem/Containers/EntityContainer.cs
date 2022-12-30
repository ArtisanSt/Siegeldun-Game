using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityContainer : SettingsSystem<EntityContainer>
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

    }

    // When turned disabled
    protected override void OnDisable()
    {
        base.OnDisable();

    }

    // When turned enabled
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    // When scene ends
    protected override void OnDestroy()
    {
        base.OnDestroy();

    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    [SerializeField] public static string ScriptableObjectsPath = "Assets/Resources/Scriptable Objects";
    [SerializeField] public int entityInstanceCap;
    public bool ignoreCap; // Debugging

    [SerializeField] public GameObject groundG;

    [System.Serializable]
    public abstract class RowPropertyEntity<TEntityProp>
        where TEntityProp : EntityProp
    {
        public bool allow;
        public int instanceCap;
        public TEntityProp entityProp;

        public EntityProp GetProp() { return (EntityProp)entityProp; }
        public virtual string type { get { return (entityProp == null) ? Globals.nullPlaceholder : entityProp.entityType.ToString(); } }
    }

    // Entity
    [System.Serializable] public class RowPropertyStructure : RowPropertyEntity<StructureProp>, IInstantiatiable { }
    [System.Serializable] public abstract class RowPropertyUnit<TUnitProp> : RowPropertyEntity<TUnitProp> where TUnitProp : UnitProp
    {
        public override string type { get { return (entityProp == null) ? Globals.nullPlaceholder : entityProp.unitType.ToString(); } }
    }
    [System.Serializable] public abstract class RowPropertyItem<TItemProp> : RowPropertyEntity<TItemProp> where TItemProp : ItemProp
    {
        public override string type { get { return (entityProp == null) ? Globals.nullPlaceholder : entityProp.itemType.ToString(); } }
    }

    public interface IInstantiatiable
    {
        public EntityProp GetProp();
        public string type { get; }
    }

    // Entity > Unit
    [System.Serializable] public class RowPropertyHuman : RowPropertyUnit<HumanProp>, IInstantiatiable { }

    // Entity > Item
    [System.Serializable] public class RowPropertyWeapon : RowPropertyItem<WeaponProp>, IInstantiatiable { }

    [System.Serializable]
    public class EntityCollections
    {
        public List<RowPropertyStructure> structures;
        public List<RowPropertyHuman> humans;
        public List<RowPropertyWeapon> weapons;

        public List<IInstantiatiable> Get(params string[] targets)
        {
            List<IInstantiatiable> temp = new List<IInstantiatiable>();
            foreach (string target in targets)
            {
                string fieldName = target.Trim().ToLower();
                if (!fieldNames.Contains(fieldName).EvaluateOr()) continue;
                if (fieldName == "structures") temp.AddRange(structures.Convert<RowPropertyStructure>());
                if (fieldName == "humans") temp.AddRange(humans.Convert<RowPropertyHuman>());
                if (fieldName == "weapons") temp.AddRange(weapons.Convert<RowPropertyWeapon>());
            }

            if (temp.Count == 0)
                return Get(fieldNames);
            else return temp;
        }

        public string[] fieldNames { get { return (from System.Reflection.FieldInfo fieldInfo in typeof(EntityCollections).GetFields() select fieldInfo.Name).ToArray(); } }
    }
    public EntityCollections collections;


    public string IdentifyType(string entityName)
    {
        Contains(entityName, out string entityType, collections.Get().ToArray());
        return entityType;
    }

    public bool Contains(string entityName, out string entityType, params IInstantiatiable[] entities)
    {
        entityType = Globals.nullPlaceholder;
        foreach (IInstantiatiable entity in entities)
        {
            if (entityName == entity.GetProp().entityName)
            {
                entityType = entity.type;
                return true;
            }
        }
        return false;
    }

    public bool Contains(string entityName, out string entityType, List<IInstantiatiable> entities)
    {
        return Contains(entityName, out entityType, entities.ToArray());
    }

    public bool Contains(string entityName, out int index, params IInstantiatiable[] entities)
    {
        index = -1;
        for (int i=0; i < entities.Length; i++)
        {
            if (entityName == entities[i].GetProp().entityName)
            {
                index = i;
                return true;
            }
        }
        return false;
    }

    public bool Contains(string entityName, out int index, List<IInstantiatiable> entities)
    {
        return Contains(entityName, out index, entities.ToArray());
    }
}

public static class InstantiatiableExtensions
{
    public static List<EntityContainer.IInstantiatiable> Convert<TSource>(this List<TSource> source) where TSource : EntityContainer.IInstantiatiable
    {
        return source.Select(x => (EntityContainer.IInstantiatiable)x).ToList();
    }

    public static List<EntityContainer.IInstantiatiable> Combine<TSource>(this List<EntityContainer.IInstantiatiable> target, List<TSource> source) where TSource : EntityContainer.IInstantiatiable
    {
        List<EntityContainer.IInstantiatiable> temp = new List<EntityContainer.IInstantiatiable>(target);
        temp.AddRange(source.Select(x => (EntityContainer.IInstantiatiable)x).ToList());
        return temp;
    }
}