using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBase : EquipmentBase
{
    protected Player p;
    public float applyTime;

    private void OnEnable()
    {
        p = manager.player;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (manager.safeToNextSwitch)
            {
                Debug.Log("primfire");
                PrimaryFire();
            }
        }

        if (isExpired)
        {
            manager.ExpireEquipment();
            isExpired = false;
        }
    }
}
