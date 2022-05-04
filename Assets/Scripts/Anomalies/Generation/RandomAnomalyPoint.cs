using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAnomalyPoint : MonoBehaviour
{
    public GameObjectSpawnList anomalyPrefabs;
    public float chanceToSpawn = 0.8f;

    private GameObject anomaly;
    private Transform p;
    
    private void Start()
    {
        p = FindObjectOfType<Player>().transform;
        if (Random.value < chanceToSpawn)
        {
            anomaly = anomalyPrefabs.GetEntry();
        }

        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        for (;;)
        {
            if(Vector3.Distance(p.position, transform.position) < Player.anomalyLoadDistance)
                anomaly.SetActive(true);
            else
                anomaly.SetActive(false);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
