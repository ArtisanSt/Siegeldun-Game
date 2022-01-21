using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Collider2D[] hitColliders { get; private set; }
    public float[] interactDistance { get; private set; }
    public LayerMask m_layerMask;
    private Vector2 center;
    private Vector2 size;

    public GameObject curSelected { get; private set; }
    private GameObject prevSelected = null;
    private bool[] _selectCalled = new bool[2] { true, true}; // prev, cur, true means done, false means select animation not called yet

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dirFacing = transform.localScale.x/Mathf.Abs(transform.localScale.x);
        center = new Vector2(transform.position.x + .3f * dirFacing, transform.position.y - .3f);
        size = new Vector2(3, 1);

        hitColliders = Physics2D.OverlapBoxAll(center, size, 0, m_layerMask);
        interactDistance = new float[hitColliders.Length];

        int[] idxNearest = new int[2] { -1, -1 }; // ,Front, Back
        float selectAlpha = transform.localScale.x * -.3f;

        for (int i=0; i < hitColliders.Length; i++)
        {
            Collider2D coll = hitColliders[i];
            float curDistance = (coll.transform.position.x - (transform.position.x + selectAlpha)) * dirFacing;
            interactDistance[i] = curDistance;

            // Items in Front of Player and colliding with the Player
            if (curDistance >= 0 && (idxNearest[0] == -1 || curDistance < interactDistance[idxNearest[0]]))
            {
                idxNearest[0] = i;
            }
            // Items behind the Player
            else if (curDistance < 0 && (idxNearest[0] == -1 || curDistance > interactDistance[idxNearest[0]]))
            {
                idxNearest[1] = i;
            }
        }

        int idxSelect = (idxNearest[0] != -1) ? idxNearest[0] : idxNearest[1];

        // No Near Item
        if (idxSelect == -1)
        {
            ChangeSelection(null);
        }
        else if (curSelected != hitColliders[idxSelect].gameObject)
        {
            ChangeSelection(hitColliders[idxSelect].gameObject);
        }

        if (!_selectCalled[0])
        {
            prevSelected.GetComponent<Interactibles>().ToggleInteract(false);
            _selectCalled[0] = true;
        }

        if (!_selectCalled[1])
        {
            curSelected.GetComponent<Interactibles>().ToggleInteract(true);
            _selectCalled[1] = true;
        }
    }

    private void ChangeSelection(GameObject newSelected)
    {
        _selectCalled[0] = curSelected == null;
        prevSelected = curSelected;

        _selectCalled[1] = newSelected == null;
        curSelected = newSelected;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
