using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
               Destroy(gameObject);
        }
    }
}
