using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraviPullZone : MonoBehaviour
{
    public float pullForce;

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if(other.attachedRigidbody.isKinematic) return;
            Vector3 dir = (transform.position - other.transform.position).normalized * pullForce;
            other.attachedRigidbody.AddForce(dir, ForceMode.Force);
        }
    }
}
