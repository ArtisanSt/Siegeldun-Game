using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsContainer : MonoBehaviour
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
    [SerializeField] public string prefabsPath = "Assets/Resources/Prefabs";
    [SerializeField] public Dictionary<string, GameObject> prefabsList = new Dictionary<string, GameObject>();


    // Reload all prefabs in the specified location
    protected void PrefabsReloader()
    {

    }
}
