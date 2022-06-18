using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector2Int storedPos;
    ItemStack keptStack;

    RectTransform rectTransform, canvasRectTransform;
    Canvas parentCanvas;
    Vector2 lastPosition, dragPosition;
    public InventorySlot droppedInto;
    bool isDragged;
    int lastRotation;

    InventoryManagerUI uiParent;
    [SerializeField] Image stackImage;
    [SerializeField] TextMeshProUGUI stackAmount;

    public void Init(InventoryManagerUI ui)
    {
        rectTransform = GetComponent<RectTransform>();
        uiParent = ui;
        parentCanvas = GetComponentInParent<Canvas>();
        canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
    }

    public void SetItem(ItemStack newStack)
    {
        keptStack = newStack;
        storedPos = newStack.invPos;
    }
    public ItemStack GetItem()
    {
        return keptStack;
    }
    public void UpdateObject()
    {
        stackImage.sprite = keptStack.item.itemIcon;
        stackAmount.gameObject.SetActive(keptStack.item.itemStackSize > 1);
        stackAmount.text = keptStack.amount.ToString();
        stackImage.rectTransform.sizeDelta = new Vector2(keptStack.item.itemSize.x * 64, keptStack.item.itemSize.y * 64);
        UpdateVisual();
        if (keptStack.equippedIn == null)
        {
            Vector2 pos = new Vector2(storedPos.x * 64 + 32 * keptStack.GetRotatedSize().x, -storedPos.y * 64 - 32 * keptStack.GetRotatedSize().y);
            rectTransform.anchoredPosition = pos;
        }
        else
        {
            transform.SetParent(keptStack.equippedIn.transform);
            rectTransform.anchoredPosition = new Vector2(10 + 32 * keptStack.GetRotatedSize().x, -10 - 32 * keptStack.GetRotatedSize().y);
        }
    }

    void UpdateVisual()
    {

        rectTransform.sizeDelta = new Vector2(keptStack.GetRotatedSize().x * 64, keptStack.GetRotatedSize().y * 64);
        stackImage.rectTransform.rotation = Quaternion.AngleAxis(keptStack.rotated * 90, Vector3.forward);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = rectTransform.anchoredPosition;
        dragPosition = rectTransform.position - Input.mousePosition;

        transform.SetParent(uiParent.tempDragParent.transform, true);

        uiParent.ClearSpace(keptStack.invPos, keptStack.GetRotatedSize());
        GetComponent<Image>().raycastTarget = false;
        lastRotation = keptStack.rotated;
        isDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = true;
        isDragged = false;

        if (droppedInto == null)
        {
            Vector2Int newPos = TransformToGrid(dragPosition + (Vector2)Input.mousePosition);
            bool isFreeSpaceUnderneath = uiParent.CheckFreeSpace(newPos, keptStack.GetRotatedSize());
            if (isFreeSpaceUnderneath)
            {
                transform.SetParent(uiParent.inventoryZone.transform, true);
                keptStack.invPos = newPos;
                uiParent.FillSpace(newPos, keptStack.GetRotatedSize());
                uiParent.UpdateItemVisual();
                if (keptStack.equippedIn != null)
                {
                    uiParent.EquipInSlot(keptStack, null);
                }
            }
            else
            {
                if (keptStack.equippedIn == null)
                {
                    transform.SetParent(uiParent.inventoryZone.transform, true);
                }
                keptStack.SetRotation(lastRotation);
                UpdateVisual();
                rectTransform.anchoredPosition = lastPosition;
            }
            return;
        }
        switch (droppedInto.itemSlotType)
        {
            case ItemSlot.ARMOR:
            case ItemSlot.MASK:
            case ItemSlot.WEAPON:
                if(!uiParent.EquipInSlot(keptStack, droppedInto))
                {
                    transform.SetParent(uiParent.inventoryZone.transform, true);
                    keptStack.SetRotation(lastRotation);
                    UpdateVisual();
                    rectTransform.anchoredPosition = lastPosition;
                }
                droppedInto = null;
                break;
        }
    }

    void Update()
    {
        if (isDragged)
        {
            if (Input.GetMouseButtonDown(1))
            {
                keptStack.Rotate();
                UpdateVisual();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = dragPosition + (Vector2)Input.mousePosition;
        transform.position = pos;
    }

    Vector2Int TransformToGrid(Vector2 input)
    {
        input = CanvasPositioningExtensions.ScreenToCanvasPosition(parentCanvas, input);
        Vector2Int newVec = new Vector2Int(Mathf.Max(0, Mathf.RoundToInt((canvasRectTransform.sizeDelta.x / 2 + input.x - keptStack.GetRotatedSize().x * 32) / 64)),
                        Mathf.Max(0, Mathf.RoundToInt((canvasRectTransform.sizeDelta.y / 2 - input.y - keptStack.GetRotatedSize().y * 32) / 64)));
        return newVec;
    }
}