using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponImpactController : MonoBehaviour
{
    public static WeaponImpactController instance;
    public GameObject impactEffect;
    public GameObject bloodImpactEffect;

    private void Awake()
    {
        instance = this;
    }

    public void CreateImpactEffect(bool type, RaycastHit point)
    {
        if(type)
            Instantiate(impactEffect, point.point, Quaternion.FromToRotation(Vector3.up, point.normal));
        else
            Instantiate(bloodImpactEffect, point.point, Quaternion.FromToRotation(Vector3.up, point.normal));
    }
}
