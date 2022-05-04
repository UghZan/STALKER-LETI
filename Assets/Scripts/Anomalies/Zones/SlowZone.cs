using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float slowFactor = 0.25f;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody r))
        {
            if (other.TryGetComponent<IMoving>(out IMoving p))
            {
                p.externalSpeedMultiplier = slowFactor;
            }
            else
                r.velocity *= slowFactor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController p))
        {
            p.externalSpeedMultiplier = 1;
        }
    }
}
