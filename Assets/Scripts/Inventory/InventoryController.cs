using System;
using Inventory.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [Header("Values")] 
        public int hotbarSlots = 2;
        public int invSlots = 2;

        public int maxHotbarSlots = 6;
        public int maxInvSlots = 16;

        [Header("Gameplay Vars")] 
        public int currentHotbarSlot = -1;
        public bool invMode = false;
        
        [Header("Storage")]
        [SerializeField] private ItemInfo[] hotbarItems = new ItemInfo[6]; //3 стандарт + 3 разгрузка
        [SerializeField] private ItemInfo[] invItems = new ItemInfo[16]; //8 стандарт + 4 рюкзак + 2х 2 подсумок
        [SerializeField] private ItemInfo[] artItems = new ItemInfo[3];

        [Header("References")] 
        public InventoryUIController ui;
        public EquipmentManager equip;
        public ArtifactBeltController art;
        public Player p;
        
        public void Start()
        {
            p = GetComponent<Player>();
            for (int i = 0; i < 4; i++)
            {
                hotbarItems[i] = null;
                invItems[i] = null;
            }
        }

        public int GetAmmoCount()
        {
            return p.hasVAL ? p.asvalammo : p.ak47ammo;
        }

        public void AddMaxInvSlots(int to)
        {
            invSlots += to;
            ui.UpdateInventorySlots();
        }

        public void AddMaxHotSlots(int to)
        {
            hotbarSlots += to;
            ui.UpdateHotbarSlots();
        }

        public bool TransferItems(int slotFrom, int slotTo) //updated version, prettier now :3
        {
            if (slotFrom == slotTo) return true;

            ItemInfo tempItem, backItem = null;
            int origin = 0; //0 - hotbar, 1 - inv, 2 - art

            //taking item
            if (slotFrom < maxHotbarSlots) //taking from hotbar slots
            {
                if (hotbarItems[slotFrom] == null) //just in case, it should be handled by InventorySlot itself
                    return false;

                tempItem = hotbarItems[slotFrom];
                origin = 0;
            }
            else if (slotFrom < maxInvSlots) //taking from inv slots
            {
                slotFrom -= maxHotbarSlots;
                if (invItems[slotFrom] == null)
                    return false;

                tempItem = invItems[slotFrom];
                origin = 1;
            }
            else //taking from art slots
            {
                slotFrom -= maxHotbarSlots + maxInvSlots;
                if (artItems[slotFrom] == null)
                    return false;

                tempItem = artItems[slotFrom];
                origin = 2;
            }

            //putting item
            if (slotTo < maxHotbarSlots)
            {
                if (hotbarItems[slotTo] != null) //putting in empty slot
                    backItem = hotbarItems[slotTo];
                hotbarItems[slotTo] = tempItem;
            }
            else if (slotTo < maxInvSlots)
            {
                slotTo -= maxHotbarSlots;
                if (invItems[slotTo] != null) //putting in empty slot
                    backItem = invItems[slotTo];
                invItems[slotTo] = tempItem;
            }
            else
            {
                if (!(tempItem is ArtifactBase)) return false;
                
                slotTo -= maxHotbarSlots + maxInvSlots;
                if (artItems[slotTo] != null)
                {
                    //putting in empty slot
                    backItem = artItems[slotTo];
                    art.RemoveArtifact(slotTo);
                }

                artItems[slotTo] = tempItem;
                art.AddArtifact((ArtifactBase)tempItem, slotTo);
            }
            
            //cleaning up
            switch (origin)
            {
                case 0:
                    hotbarItems[slotFrom] = backItem;
                    break;
                case 1:
                    invItems[slotFrom] = backItem;
                    break;
                case 2:
                    artItems[slotFrom] = backItem;
                    art.RemoveArtifact(slotFrom);
                    break;
            }

            ui.UpdateHotbarSlots();
            ui.UpdateInventorySlots();
            ui.UpdateArtSlots();
            return true;
        }

        public bool CheckForFreeSpace()
        {
            if (GetFreeHotbarSlot() != -1 || GetFreeInvSlot() != -1)
            {
                return true;
            }
            return false;
        }
        
        public bool AddItem(ItemInfo newItem)
        {
            if(currentHotbarSlot >= 0 && currentHotbarSlot < 4)
                if (hotbarItems[currentHotbarSlot] == null)
                {
                    hotbarItems[currentHotbarSlot] = newItem;
                    equip.SwitchEquipment(hotbarItems[currentHotbarSlot].itemEquipmentID);
                    ui.UpdateHotbarSlots();
                    return true;
                }

            int hotbarSlot = GetFreeHotbarSlot();
            if (hotbarSlot != -1)
            {
                hotbarItems[hotbarSlot] = newItem;
                ui.UpdateHotbarSlots();
                return true;
            }
            else
            {
                int invSlot = GetFreeInvSlot();
                if (invSlot != -1)
                {
                    invItems[invSlot] = newItem;
                    ui.UpdateInvItem(invSlot);
                    return true;
                }
            }
            return false;

        }

        public int GetFreeHotbarSlot()
        {
            for (int i = 0; i < hotbarSlots; i++)
            {
                if (hotbarItems[i] == null)
                    return i;
            }

            return -1;
        }
    
        public int GetFreeInvSlot()
        {
            for (int i = 0; i < invSlots; i++)
            {
                if (invItems[i] == null)
                    return i;
            }

            return -1;
        }

        public bool ChangeToHotbarSlot(int num)
        {
            if (num > hotbarSlots - 1 || num < 0)
                return false;

            if (currentHotbarSlot == num)
            {
                equip.HideEquipment(currentHotbarSlot);
                currentHotbarSlot = -1;
                ui.UpdateSelector(-1);
                return false;
            }

            currentHotbarSlot = num;
            ui.UpdateSelector(num);
            if(hotbarItems[currentHotbarSlot] != null)
                equip.SwitchEquipment(hotbarItems[currentHotbarSlot].itemEquipmentID);
            return true;
        }
        
        //for bolt and weapon
        public bool ChangeToSpecialSlot(int num)
        {
            //60 = bolt, 61 = aksu, 62 = val
            if (num != 60 && num != 61 && num != 62)
                return false;
            
            if (currentHotbarSlot == num)
            {
                if(currentHotbarSlot != -1) equip.HideEquipment(currentHotbarSlot);
                currentHotbarSlot = -1;
                ui.UpdateSelector(-1);
                return false;
            }
            
            currentHotbarSlot = num;
            equip.SwitchEquipment(num);
            return true;
        }

        public ItemInfo GetItemInHotbarSlot(int id)
        {
            if(id >= 0 && id < maxHotbarSlots)
                if (hotbarItems[id] != null)
                    return hotbarItems[id];
            return null;
        }
        
        public ItemInfo GetItemInInvSlot(int id)
        {
            if(id >= 0 && id < maxInvSlots)
                if (invItems[id] != null)
                    return invItems[id];
            return null;
        }
        
        public ItemInfo GetItemInArtSlot(int id)
        {
            if(id >= 0 && id < 3)
                if (artItems[id] != null)
                    return artItems[id];
            return null;
        }
        
        public ItemInfo GetCurrentSelectedItem()
        {
            return GetItemInHotbarSlot(currentHotbarSlot);
        }
        
        public void DiscardItemInHotbarSlot(int id)
        {
            if (id >= 0 && id < maxHotbarSlots)
            {
                hotbarItems[id] = null;
                ui.UpdateHotbarItem(id);
            }
        }
        public void DiscardItemInInvSlot(int id)
        {
            if (id >= 0 && id < maxInvSlots)
            {
                invItems[id] = null;
                ui.UpdateInvItem(id);
            }
        }
        
        public void DiscardItemInArtSlot(int id)
        {
            if (id >= 0 && id < 3)
            {
                artItems[id] = null;
                ui.UpdateArtItem(id);
            }
        }
        
        public void DiscardSelectedItem()
        {
            DiscardItemInHotbarSlot(currentHotbarSlot);
        }

        public void ControlInvMode(bool state)
        {

                invMode = state;
                ui.ChangeInventoryUIVisibility(state);
        }
    }
}
