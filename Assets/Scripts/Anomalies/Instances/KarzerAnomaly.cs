using System.Collections;
using System.Collections.Generic;
using Anomalies;
using UnityEngine;
using UnityEngine.EventSystems;

public class KarzerAnomaly : AnomalyBase
{
    public float nextFreezeTimer;
    public ParticleSystem freezeSystem;
    public GameObject damageZone;

    private float thawTimer;
    private bool frozen;

    public void Start()
    {
        nextFreezeTimer = Random.Range(30, 120);
    }
   
    protected override void Update()
    {
        base.Update();
        if (nextFreezeTimer <= 0 && !frozen)
        {
            frozen = true;
            freezeSystem.Play();
            damageZone.SetActive(true);
            thawTimer = Random.Range(10, 30);
        }

        if (!frozen)
        {
            nextFreezeTimer -= Time.deltaTime;
        }
        else
        {
            thawTimer -= Time.deltaTime;
        }

        if (thawTimer <= 0 && frozen)
        {
            frozen = false;
            freezeSystem.Stop();
            damageZone.SetActive(false);
            nextFreezeTimer = Random.Range(30, 120);
        }
    }

    protected override void OnObjectStay(Collider other)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
