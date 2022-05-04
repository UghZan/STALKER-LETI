using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Inventory.UI
{
   public class InventoryUIController : MonoBehaviour
   {
      public InventoryController inv;

      [Header("Drag&Drop")] 
      public GameObject dragDummy;
      public bool dragging = false;
      public int startDragSlot;
      public int endDragSlot;

      [Header("Tooltip")] public GameObject tooltip;
      public Text tooltipName;
      public Text tooltipDesc;
      public Vector3 offset;
      
      [Header("References")] 
      public GameObject invUI;
      public Text ammoCounter;
      public InventorySlot[] uiHotbarSlots;
      public InventorySlot[] uiInvSlots;
      public InventorySlot[] uiArtSlots;
      public Image selector;

      public void Start()
      {
         for (int i = 0; i < uiHotbarSlots.Length; i++)
         {
            uiHotbarSlots[i].slotID = i;
         }

         for (int i = 0; i < uiInvSlots.Length; i++)
         {
            uiInvSlots[i].slotID = i + uiHotbarSlots.Length;
         }
         
         for (int i = 0; i < uiArtSlots.Length; i++)
         {
            uiArtSlots[i].slotID = i + uiInvSlots.Length + uiHotbarSlots.Length;
         }
         UpdateHotbarSlots();
         UpdateInventorySlots();
         UpdateArtSlots();
      }

      public void Update()
      {
         if (dragging)
         {
            dragDummy.transform.position = Input.mousePosition;
         }
         else
         {
            tooltip.transform.position = Input.mousePosition + offset;
         }

         if (dragging && Input.GetMouseButtonDown(1))
         {
            dragging = false;
            dragDummy.SetActive(false);
         }

         ammoCounter.text = inv.GetAmmoCount().ToString();
      }

      public void ChangeInventoryUIVisibility(bool state)
      {
         UpdateInventorySlots();
         invUI.SetActive(state);
         if (!state && dragging) //there's dragging in process
         {
            dragging = false;
            dragDummy.SetActive(false);
         }
      }
      
      public void UpdateSelector(int id)
      {
         if(id == -1)
            selector.gameObject.SetActive(false);
         else
         {
            selector.gameObject.SetActive(true);
            if(!uiHotbarSlots[id].gameObject.activeInHierarchy) return;
            else
            {
               selector.rectTransform.position = uiHotbarSlots[id].GetComponent<RectTransform>().position;
            }
         }
      }
      
      public void UpdateHotbarItem(int id)
      {
         uiHotbarSlots[id].keptItem = inv.GetItemInHotbarSlot(id);
         uiHotbarSlots[id].UpdateIcon();
      }
      
      public void UpdateInvItem(int id)
      {
         uiInvSlots[id].keptItem = inv.GetItemInInvSlot(id);
         uiInvSlots[id].UpdateIcon();
      }
      
      public void UpdateArtItem(int id)
      {
         uiArtSlots[id].keptItem = inv.GetItemInArtSlot(id);
         uiArtSlots[id].UpdateIcon();
      }
      
      public void UpdateHotbarSlots()
      {
         for (int i = 0; i < inv.maxHotbarSlots; i++)
         {
            if (uiHotbarSlots[i].ui == null) uiHotbarSlots[i].ui = this;
            if (i < inv.hotbarSlots)
            {
               if (!uiHotbarSlots[i].gameObject.activeInHierarchy) uiHotbarSlots[i].gameObject.SetActive(true);
               UpdateHotbarItem(i);
            }
            else
               uiHotbarSlots[i].gameObject.SetActive(false);
            
         }
      }
      
      public void UpdateInventorySlots()
      {
         for (int i = 0; i < inv.maxInvSlots; i++)
         {
            if (uiInvSlots[i].ui == null) uiInvSlots[i].ui = this;
            if (i < inv.invSlots)
            {
               if (!uiInvSlots[i].gameObject.activeInHierarchy) uiInvSlots[i].gameObject.SetActive(true);
               UpdateInvItem(i);
            }
            else
               uiInvSlots[i].gameObject.SetActive(false);
            
         }
      }
      
      public void UpdateArtSlots()
      {
         for (int i = 0; i < 3; i++)
         {
            UpdateArtItem(i);
         }
      }
   }
}
