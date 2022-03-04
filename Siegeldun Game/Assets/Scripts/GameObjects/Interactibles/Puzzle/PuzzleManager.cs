using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public List<GameObject> torches, buttons, gates;
    public bool puzzleStart;

    [SerializeField] private AudioClip gateOpenSfx;

    private void Start()
    {
        Instance = this;
        puzzleStart = false;

        gates[0].GetComponent<Animator>().SetTrigger("Open");
        gates[0].GetComponent<BoxCollider2D>().enabled = false;
        gates[1].GetComponent<Animator>().SetTrigger("Close");
        gates[1].GetComponent<BoxCollider2D>().enabled = true;
    }


    public bool isAllOn()
    {
        foreach(GameObject torch in torches)
        {
            if(!torch.GetComponent<TorchScript>().isOn)
                return false;
        }
        return true;
    }

    public void CloseGates()
    {
        if (!puzzleStart)
        {
            foreach(GameObject gate in gates)
            {
                gate.GetComponent<Animator>().SetTrigger("Close");
                gate.GetComponent<BoxCollider2D>().enabled  = true;
            }
            puzzleStart = true;
        }
    }

    public void OpenGates()
    {
        Debug.Log("Open Gates");
        SoundManager.instance.PlaySFX(gateOpenSfx);

        foreach(GameObject button in buttons)
        {
            button.GetComponent<PuzzleButton>().ToggleInteractible();
            button.GetComponent<SpriteRenderer>().material.DisableKeyword("OUTLINE_ON");
        }

        foreach(GameObject gate in gates)
        {
            gate.GetComponent<Animator>().SetTrigger("Open");
            gate.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
