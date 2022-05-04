using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsStopZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody r)) {r.isKinematic = true; r.velocity = Vector3.zero;}
    }
}
