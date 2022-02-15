using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpikes : Structures
{
    // ========================================= Structure Initialization =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "WoodSpikes";
    public override string objectName { get { return _objectName; } }

    private string _structureName = "Wood Spikes";
    public override string structureName { get { return _structureName; } }*/


    [Header("SPIKES SETTINGS", order = 3)]
    [SerializeField] protected WeaponProperties spikesPower = new WeaponProperties();
    [SerializeField] protected List<LayerMask> damageableLayers;
    private Dictionary<GameObject, bool> entitiesInside = new Dictionary<GameObject, bool>();
    [SerializeField] private float continuousDmgDelay = 0f;

    protected override void Awake()
    {
        base.Awake();
        isInteractible = false;
    }

    protected void Update()
    {
        Dictionary<GameObject, bool> collidingEntities = new Dictionary<GameObject, bool>();
        foreach (KeyValuePair<GameObject, bool> entityInside in entitiesInside)
        {
            collidingEntities.Add(entityInside.Key, entityInside.Value);
        }

        foreach (KeyValuePair<GameObject, bool> entityInside in collidingEntities)
        {
            if (!entityInside.Value) continue;

            float attackID = (float)gameObject.GetInstanceID() + Random.Range(-9999, 10000) / 10000;
            GameObject entity = entityInside.Key;
            entity.GetComponent<IDamageable>().TakeDamage(attackID, entity.GetComponent<IFaceScaling>().spriteFacing, spikesPower);
            entitiesInside[entityInside.Key] = false;
            StartCoroutine(ContinuousDamageTimer(entity));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        bool inAffectedLayer = damageableLayers.Contains(LayerMask.GetMask(LayerMask.LayerToName(col.gameObject.layer)));
        if (!inAffectedLayer || col.gameObject.GetComponent<IDamageable>() == null || entitiesInside.ContainsKey(col.gameObject)) return;
        entitiesInside.Add(col.gameObject, true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!entitiesInside.ContainsKey(col.gameObject)) return;
        entitiesInside.Remove(col.gameObject);
    }

    private IEnumerator ContinuousDamageTimer(GameObject entity)
    {
        yield return new WaitForSeconds(continuousDmgDelay);
        if (entitiesInside.ContainsKey(entity)) { entitiesInside[entity] = true; }
    }
}
