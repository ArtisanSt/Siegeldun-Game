using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataProp : SettingsSystem<DataProp>
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


    // ============================== MAIN METHODS ==============================
    public void DataInit()
    {

    }


    // ============================== MAIN PROPERTIES ==============================
    public List<UnitProp> unitProps;
    public List<ItemProp> itemProps;
    public List<StructureProp> structureProps;

    public List<EntityProp> entityProps
    {
        get
        {
            List<EntityProp> temp = new List<EntityProp>();
            temp.AddRange(unitProps.Select(x => (EntityProp)x).ToList());
            temp.AddRange(itemProps.Select(x => (EntityProp)x).ToList());
            temp.AddRange(structureProps.Select(x => (EntityProp)x).ToList());
            return temp;
        }
    }

    public TDataProp Get<TDataProp>(string dataName)
        where TDataProp : EntityProp
    {
        foreach (EntityProp entityProp in entityProps)
        {
            if (entityProp.entityName.Trim() == dataName.Trim())
                return (TDataProp)entityProp;
        }
        return null;
    }

    /*public UnitProp FindUnit(string unitName)
    {
        foreach (UnitProp unitProp in unitProps)
        {
            if (unitProp.unitName == unitName) return unitProp;
        }
        return null;
    }

    public UnitProp FindItem(string unitName)
    {
        foreach (UnitProp unitProp in unitProps)
        {
            if (unitProp.unitName == unitName) return unitProp;
        }
        return null;
    }

    public UnitProp FindStructure(string unitName)
    {
        foreach (UnitProp unitProp in unitProps)
        {
            if (unitProp.unitName == unitName) return unitProp;
        }
        return null;
    }*/
}
