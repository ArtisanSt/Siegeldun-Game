using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = FindInstance<T>();
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

    public static TOutput FindInstance<TOutput>() where TOutput: Object
    {
        return (TOutput)FindObjectOfType(typeof(TOutput));
    }
}
