using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GOSpawnEntry
{
    public GameObject obj;
    public int weight = 100;
}

[CreateAssetMenu(menuName = "GameObject Spawn List")]
public class GameObjectSpawnList : ScriptableObject
{
    public new GOSpawnEntry[] list;
    public int maxWeight { get; private set; }

    private void Start()
    {
        foreach (var obe in list)
        {
            maxWeight += obe.weight;
        }
    }
    
    public GameObject GetEntry()
    {
        int weight = Random.Range(0, maxWeight);

        for (int i = 0; i < list.Length; i++)
        {
            weight -= list[i].weight;
            if(weight <= 0)
                return list[i].obj;
        }

        return null;
    }
}
