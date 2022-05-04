using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        public bool pickedUp;

        public int special = -1;
        
        public ItemInfo keptItem;
        public Player owner; //used to know where is the player that picked it up

        public void UpdateKeptItem(ItemInfo newItem)
        {
            keptItem = newItem;
        }

        private void Update()
        {
            if (pickedUp)
            {
                transform.position = Vector3.Lerp(transform.position, owner.pickupZone.bounds.center, 5f * Time.deltaTime);
            }
        }

        public void OnInteract(Player player)
        {
            if(!player.inv.CheckForFreeSpace())
                return;
            
            if (!pickedUp)
            {
                pickedUp = true;
                owner = player;
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player p) && pickedUp)
            {
                if(special == -1)
                    p.inv.AddItem(keptItem);
                else
                    p.UpdateSpecials(special);
                Destroy(gameObject);
            }
        }
    }
}
