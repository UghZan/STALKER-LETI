using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemSlot itemSlotType;
    ItemStack keptItem = null;
    public bool equipped;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<ItemObject>().droppedInto = this;
        }
    }

    public void SetItem(ItemStack stack)
    {
        keptItem = stack;
        equipped = stack != null;
    }

    public ItemStack GetItem()
    {
        return keptItem;
    }
}
