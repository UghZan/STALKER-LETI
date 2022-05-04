using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public float damage;
    public DamageType dmgType;
    
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    
    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        IDamageable rb = other.GetComponent<IDamageable>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb != null)
            {
                rb.TakeDamage(dmgType, damage);
            }
            i++;
        }
    }
}
