using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Settings<T> : MonoBehaviour, ISingleton where T: MonoBehaviour
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {
        InstanceConfiguration();
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


    // ============================== SINGLETON PROPERTIES AND METHODS ==============================
    public static T instance { get; private set; }

    public void InstanceConfiguration()
    {
        instance = GameSystem.FindInstance<T>();
    }
}
