using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class MossFruit : MonoBehaviour
{
    private MossAnomaly parent;
    
    public float maxAge;
    public bool willBeRipe;
    public float baseDamage = 5f;
    
    private float age;

    public void Initialize(MossAnomaly _par, bool _ripe, float _age)
    {
        parent = _par;
        willBeRipe = _ripe;
        age = _age;
        transform.localScale *= 0.5f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (age > maxAge * 0.33f)
        {
            parent.CreateSporeExplosion(transform.position);
            parent.MossFruitDied();
            Destroy(gameObject);
        }
    }

    IEnumerator Aging()
    {
        if(age < maxAge) age++;
        else {parent.CreateSporeExplosion(transform.position); parent.MossFruitDied(); Destroy(gameObject); }
        float ageScale = 0.5f + age / maxAge;
        transform.localScale = new Vector3(ageScale,ageScale,ageScale);
        yield return new WaitForSeconds(1f);
    }
}
