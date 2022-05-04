using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour
{
    //primary fire is LMB, secondary is RMB+LMB

    public bool isExpired;
    [SerializeField] protected EquipmentManager manager;

    protected virtual void PrimaryFire()
    {
    }

    protected virtual void SecondaryFire()
    {
    }
}
