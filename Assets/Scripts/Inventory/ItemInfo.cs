using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
    public class ItemInfo : ScriptableObject
    {
        public string itemName;
        public string itemDesc;
        public Sprite itemIcon;
        public int itemEquipmentID;
    }
}
