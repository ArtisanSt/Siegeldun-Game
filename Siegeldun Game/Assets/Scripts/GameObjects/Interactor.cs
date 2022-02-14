using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : Process
{
    private Collider2D[] hitColliders;
    private float[] interactDistance;
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private Vector2 center;
    private Vector2 _center;
    [SerializeField] private Vector2 size; //3,1

    public GameObject curSelected { get; private set; }
    private GameObject prevSelected = null;
    [SerializeField] private bool outlined = false; 
    private bool[] _selectCalled = new bool[2] { true, true}; // prev, cur, true means done, false means select animation not called yet

    private void InteractorFixedUpdate()
    {
        float dirFacing = transform.localScale.x / Mathf.Abs(transform.localScale.x);
        _center = new Vector2(transform.position.x + this.center.x * dirFacing, transform.position.y + this.center.y); // +.3f, -.3f

        hitColliders = Physics2D.OverlapBoxAll(_center, size, 0, m_layerMask);
        interactDistance = new float[hitColliders.Length];

        int[] idxNearest = new int[2] { -1, -1 }; // ,Front, Back
        float selectAlpha = transform.localScale.x * -.3f;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<IInteractible>() == null || !hitColliders[i].GetComponent<IInteractible>().isInteractible || !gameObject.GetComponent<IInteractor>().InteractorColliderConditions(hitColliders[i])) continue;

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
            curSelected.GetComponent<Interactibles>().ToggleInteract(true && outlined);
            _selectCalled[1] = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IgnoreErrors(InteractorFixedUpdate);
    }

    private void ChangeSelection(GameObject newSelected)
    {
        _selectCalled[0] = curSelected == null;
        prevSelected = curSelected;

        _selectCalled[1] = newSelected == null;
        curSelected = newSelected;
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_center, size);
    }
    */
}
