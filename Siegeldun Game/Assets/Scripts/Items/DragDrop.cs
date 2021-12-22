using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private InventorySystem inventory;

    Vector3 defaultPos;
    public bool droppedOnSlot;

    private void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<InventorySystem>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        defaultPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        eventData.pointerDrag.GetComponent<DragDrop>().droppedOnSlot = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (droppedOnSlot == false)
        {
            if(eventData.pointerDrag.GetComponent<Item>().itemName == inventory.consumeSlot)
            {
                inventory.consumeSlot = null;
                Destroy(GameObject.Find("SlotB_Item"));
            }
            if(eventData.pointerDrag.GetComponent<Item>().itemName == inventory.weaponSlot)
            {
                inventory.weaponSlot = null;
                Destroy(GameObject.Find("SlotA_Item"));
            }
            transform.position = defaultPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
}
