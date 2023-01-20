using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Vector2 centerShift;
    private Vector2 center => new Vector2(transform.position.x, transform.position.y) + centerShift;
    public Vector2 size;
    private LayerMask interactLayers;
    public float frontDistancePriority;

    public GameObject curSelected { get; private set; }
    private GameObject prevSelected;

    private float dirFacing => transform.localScale.x / Mathf.Abs(transform.localScale.x);

    private void SelectInteractible()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0, interactLayers);
        float[] collDistances = new float[hitColliders.Length];

        Vector2Int idxNearest = new Vector2Int(-1, -1); // Front, Back
        Vector2 nearestDistance = new Vector2(-1, 1); // Front, Back
        float selectAlpha = transform.localScale.x * -.3f;

        for (int i=0; i<hitColliders.Length; i++)
        {
            Collider2D coll = hitColliders[i];
            if (coll.GetComponent<IInteractible>() == null || !coll.GetComponent<IInteractible>().allowed) continue;

            float distance = (coll.transform.position.x - transform.position.x) * dirFacing;
            collDistances[i] = distance;

            if (distance.Positive().Min(nearestDistance.x) == distance)
            {
                idxNearest.Set(i, idxNearest.y);
                nearestDistance.Set(distance, nearestDistance.y);
            }
            else if (distance.Negative().Max(nearestDistance.y) == distance)
            {
                idxNearest.Set(i, idxNearest.y);
                nearestDistance.Set(distance, nearestDistance.y);
            }
        }

        int idxSelect = (idxNearest.x != -1) ? idxNearest.x : idxNearest.y;

        if (idxSelect == -1) Select(null);
        else Select(hitColliders[idxSelect].gameObject);
    }

    public void Select(GameObject newSelected)
    {
        if (curSelected == newSelected) return;

        if (curSelected != null)
        {
            curSelected.GetComponent<IInteractible>().selected = false;
            prevSelected = curSelected;
        }

        curSelected = newSelected;
        newSelected.GetComponent<IInteractible>().selected = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
