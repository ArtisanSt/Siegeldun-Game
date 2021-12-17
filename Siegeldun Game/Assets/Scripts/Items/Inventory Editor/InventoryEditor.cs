using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(InventorySystem))]
public class InventoryEditor : Editor
{
    Dictionary<string, int> inventoryItem;
    private bool INVENTORY = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        INVENTORY = EditorGUILayout.Foldout (INVENTORY, "Show Inventory");

        if(INVENTORY)
        {
            inventoryItem = new Dictionary<string, int>();
            InventorySystem myInventoryScript = (InventorySystem)target;
            inventoryItem = myInventoryScript.inventoryItems;
            if(inventoryItem != null)
            {
                foreach (KeyValuePair<string, int> kvp in inventoryItem)
                {
                    EditorGUILayout.IntField(kvp.Key.ToString(), kvp.Value);
                }
            }
        }
    }
}