using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GummyZone : MonoBehaviour
{
    public GameObject actingZone;
    public float sizeMultiplier = 1.0f;

    void Start()
    {
        sizeMultiplier = Random.Range(0.5f, 2f);
        actingZone.GetComponent<SphereCollider>().radius *= sizeMultiplier;
    }
}
