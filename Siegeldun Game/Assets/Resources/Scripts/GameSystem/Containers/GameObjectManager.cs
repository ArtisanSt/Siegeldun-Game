using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameObjectManager
{
    /*public static GameObject Instantiate(string entityName, Vector3 position)
    {
        if (entityName.Trim().FoundIn("", Globals.nullPlaceholder)) return null;

        GameObject entityG = BaseGameObject(entityName);
        GameObject childG = CreateInstance(entityG, position);
        return childG;
    }

    public static GameObject Duplicate(GameObject entityG, Vector3 position)
    {
        if (entityG == null) return null;

        GameObject childG = CreateInstance(entityG, position);
        return childG;
    }

    public static GameObject CreateInstance(GameObject entityG, Vector3 position)
    {
        return (GameObject)Object.Instantiate(entityG, position, Quaternion.identity, entityG.GetComponent<Entity<EntityProp>>().parentT);
    }


    private static GameObject BaseGameObject(string entityName)
    {
        string entityType = EntityContainer.instance.IdentifyType(entityName);
        if (entityType == Globals.nullPlaceholder) return null;


        GameObject entityG = new GameObject("Script Creation");
        entityG.name = entityName;
        entityG.AddComponent(System.Type.GetType(entityType));
        ComponentDependencies(ref entityG);

        return entityG;
    }

    // Additional Components
    private static void ComponentDependencies(ref GameObject gameObject)
    {

    }*/
}
