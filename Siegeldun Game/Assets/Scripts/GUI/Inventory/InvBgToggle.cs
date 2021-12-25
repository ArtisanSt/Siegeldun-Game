using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvBgToggle : MonoBehaviour
{
    public bool isOn = false;
    public GameObject inventoryBg;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.transform.localRotation);
        inventoryBg = GameObject.Find("InventoryBackground");
        inventoryBg.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, (isOn) ? 90 : -90);
        inventoryBg.SetActive(isOn);
    }

    public void ToggleButton()
    {
        isOn = !isOn;
    }
}
