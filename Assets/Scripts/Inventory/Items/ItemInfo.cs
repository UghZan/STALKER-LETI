using UnityEngine;

public enum ItemSlot
{
    COMMON,
    ARMOR,
    MASK,
    WEAPON, //main weapons
    ACCESORY, //grenades and shit
}
[CreateAssetMenu(menuName = "Items/New Item")]
public class ItemInfo : ScriptableObject
{
    [Header("Visual Parameters")]
    public string itemDisplayName;
    public string itemDescription;
    public GameObject itemObject;
    public Sprite itemIcon;
    public Vector2Int itemSize;

    [Header("Inventory Parameters")]
    public float itemWeight;
    public ItemSlot itemSlot;
    public int itemStackSize = 99;
    public bool essential = false; //essential items CANNOT be discarded
}

