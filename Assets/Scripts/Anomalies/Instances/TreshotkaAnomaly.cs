using System;
using System.Collections;
using System.Collections.Generic;
using Anomalies;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class TreshotkaAnomaly : AnomalyBase
{
    [Header("References")]
    public ParticleSystem electro;
    public LineRenderer[] lightnings;
    
    [Header("Gameplay")]
    public float maxCharge = 100f;
    public float chargeSpeed = 0.1f;
    public float randomThunderVariation = 0.5f;
    
    [SerializeField] private float charge;
    private ParticleSystem.MainModule electroMain;

    [Header("Damage")] public float baseDamage;
    public DamageType damageType;
    
    private void Start()
    {
        electroMain = electro.main;
        for (int i = 0; i < 4; i++)
        {
            lightnings[i].gameObject.SetActive(false);
        }
    }


    protected override void Update()
    {
        base.Update();
        if(charge < maxCharge) charge += chargeSpeed * (1 / (charge + 0.01f) + 0.4f) * Time.deltaTime;
        electroMain.startLifetime = 0.0001f * charge * charge + 0.0017f * charge + 0.02f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(charge < 15) return;
        
        int power = (int)Mathf.Ceil(charge / 30);
        power = Mathf.Min(power, 4);
        for (int i = 0; i < power; i++)
        {
            lightnings[i].gameObject.SetActive(true);
            lightnings[i].SetPosition(0, transform.position + Random.onUnitSphere * 3);
            lightnings[i].SetPosition(7, other.transform.position);
            for (int j = 1; j < 7; j++)
            {
                lightnings[i].SetPosition(j, Vector3.Lerp(lightnings[i].GetPosition(0), lightnings[i].GetPosition(7), (float)j/6));
                lightnings[i].SetPosition(j, lightnings[i].GetPosition(j) + Random.insideUnitSphere * randomThunderVariation);
            }
        }

        if (other.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(damageType, baseDamage * charge);
        }

        charge = Random.Range(0, 10);
        
        StartCoroutine(HideThunderEffect());
    }

    private IEnumerator HideThunderEffect()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 4; i++)
        {
            GameObject go = lightnings[i].gameObject;
            if(go.activeInHierarchy) go.SetActive(false);
        }
    }
}
