using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class MiscHelper : MonoBehaviour
{
    public static MiscHelper instance;

    public GameObject pickupPrefab;

    public void Awake()
    {
        if(instance != null)
            Destroy(instance);
        
        instance = this;
    }

    public void GeneratePickupAtPosition(ItemInfo item, Vector3 pos)
    {
        GameObject _pickup = Instantiate(pickupPrefab, pos, Quaternion.identity);
        ItemPickup itemPickup = _pickup.GetComponent<ItemPickup>();
        itemPickup.UpdateKeptItem(item);
    }

    public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        x = Mathf.Clamp(x, in_min, in_max);
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
        
}
