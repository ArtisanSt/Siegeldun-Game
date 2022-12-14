using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameObjectManager
{
    private static string SOPath { get { return EntityContainer.ScriptableObjectsPath; } }

    public static GameObject Duplicate(GameObject entityG, Vector3 position)
    {
        if (entityG == null) return null;

        GameObject childG = (GameObject)Object.Instantiate(entityG, position, Quaternion.identity, entityG.GetComponent<Entity<EntityProp>>().parentT);
        return childG;
    }

    public static GameObject Instantiate(string entityName, Vector3 position)
    {
        if (entityName.Trim() == "") return null;

        GameObject entityG = BaseGameObjects(entityName);

        GameObject childG = (GameObject)Object.Instantiate(entityG, position, Quaternion.identity, entityG.GetComponent<Entity<EntityProp>>().parentT);
        return childG;
    }

    // T = UnitProp/ItemProp/StructureProp
    private static T SOInstanceLoader<T>(string entityName) where T: notnull, EntityProp
    {
        string targetName = AssetDatabase.FindAssets(entityName, new string[] { SOPath })[0];
        string targetPath = AssetDatabase.GUIDToAssetPath(targetName);
        T SOEntity = AssetDatabase.LoadAssetAtPath<T>(targetPath);

        return SOEntity;
    }

    private static GameObject BaseGameObjects(string entityName)
    {
        string[] entityTypes = EntityContainer.IdentifyType(entityName);
        if (entityTypes.Length == 0) return null;
        
        GameObject entityG = new GameObject("Script Creation");
        entityG.name = entityName;
        switch (entityTypes[0])
        {
            case "Unit":
                if (entityTypes[1] == "Human")
                    entityG = EntityComponentSetup<HumanProp, Unit<HumanProp>>(entityG);
                break;

            case "Item":
                if (entityTypes[1] == "Weapon")
                    entityG = EntityComponentSetup<WeaponProp, Item<WeaponProp>>(entityG);
                break;

            case "Structure":
                entityG = EntityComponentSetup<StructureProp, Structure<StructureProp>>(entityG);
                break;
        }



        return entityG;
    }

    private static GameObject EntityComponentSetup<N, T>(GameObject entityG)
        where N : EntityProp
        where T : Entity<N>
    {
        // When this component is attached, all of the other required component will be attached, too
        entityG.AddComponent<T>();
        entityG.GetComponent<T>().entityProp = SOInstanceLoader<N>(entityG.name);

        // Component Setup

        return entityG;
    }
}
