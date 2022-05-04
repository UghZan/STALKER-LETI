using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public int slotID; //0-5 hotbar 6-15 inventory
        public ItemInfo keptItem;
        public Image itemIcon;

        public InventoryUIController ui;
        
        public void UpdateItem(ItemInfo newItem)
        {
            keptItem = newItem;
        }

        public void UpdateIcon()
        {
            if (keptItem != null)
            {
                itemIcon.gameObject.SetActive(true);
                itemIcon.sprite = keptItem.itemIcon;
            }
            else
            {
                itemIcon.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("clicked on slot id " + slotID);
            if (!ui.dragging)
            {
                if(keptItem == null)
                    return;
                
                ui.startDragSlot = slotID;
                ui.dragging = true;
                ui.dragDummy.SetActive(true);
                ui.dragDummy.GetComponent<Image>().sprite = keptItem.itemIcon;
            }
            else
            {
                ui.endDragSlot = slotID;
                if (ui.inv.TransferItems(ui.startDragSlot, ui.endDragSlot))
                {
                    ui.dragging = false;
                    ui.dragDummy.SetActive(false);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(keptItem == null) return;
            
            ui.tooltip.SetActive(true);
            ui.tooltipName.text = keptItem.itemName;
            ui.tooltipDesc.text = keptItem.itemDesc;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.tooltip.SetActive(false);
        }
    }
}
