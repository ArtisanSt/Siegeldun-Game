using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    public bool isOn;

    public void switchTorch()
    {
        isOn = !isOn;
        transform.GetComponent<SpriteRenderer>().enabled = isOn;
    }
}
