using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolProjectile : BasicProjectile
{
    public GameObject trail;
    public GameObject impactEffect;

    protected override void OnImpact(Collider other)
    {
        impactEffect.transform.parent = null;
        impactEffect.SetActive(true);
        if (trail != null)
        {
            trail.transform.parent = null;
            trail.GetComponent<ParticleSystem>().Stop();
        }
        Destroy(gameObject);
    }
}
