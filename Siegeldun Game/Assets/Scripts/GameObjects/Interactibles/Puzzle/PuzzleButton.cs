using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : Structures
{
    [SerializeField] List<GameObject> torches;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    // Interaction Event
    public override void Interact()
    {
        if (!isSelected) return;

        transform.GetComponent<Animator>().SetTrigger("Press");

        foreach(GameObject torch in torches)
        {
            torch.GetComponent<TorchScript>().switchTorch();
        }

        if(!PuzzleManager.Instance.isAllOn())
            PuzzleManager.Instance.CloseGates();
        else
            PuzzleManager.Instance.OpenGates();
        
    }
}
