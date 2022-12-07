using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = FindInstance<T>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Update()
    {
        if (instance == null)
        {
            instance = FindInstance<T>();

            if (instance != gameObject) Destroy(gameObject);
        }
    }

    public static TOutput FindInstance<TOutput>() where TOutput : Object
    {
        return (TOutput)FindObjectOfType(typeof(TOutput));
    }
}
