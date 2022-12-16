using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectExtensions
{

}

public static class TransformExtensions
{
    public static Transform GetChild(this Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name) return child;
        }
        return null;
    }

    public static System.Func<Transform, int> childCount = transform => transform.childCount;
}

public static class GameObjectExtensions
{
    public static GameObject GetChild(this GameObject gameObject, string name)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name == name) return child.gameObject;
        }
        return null;
    }

    public static int ChildCount(this GameObject gameObject)
    {
        return gameObject.transform.childCount;
    }
}
