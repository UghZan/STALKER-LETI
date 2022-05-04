using System.Collections;
using System.Collections.Generic;
using Anomalies.Instances.Additional;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MossAnomaly : MonoBehaviour
{
    [Header("Moss Cluster Settings")]
    public int mossClusters;
    public int mossPlanesPerCluster;
    public float mossTryDistance;
    public float minMossRadius;
    public float maxMossRadius;
    public GameObject mossCluster;

    [Header("Moss Vines Settings")] 
    public int mossVinesAmount;
    public float mossVinesVariance;
    public float minVineDistance;
    public GameObject mossVine;

    [Header("Moss Fruit Settings")] public GameObject mossFruit;
    public float mossFruitCooldown;
    public float maxMossFruits;
    public float chanceToDrop;
    public ParticleSystem sporeExplosion;

    private List<GameObject> mossClusterArray;
    private List<LineRenderer> mossVinesArray;
    private int mossFruits;
    private float mossFruitTimer;
    private ParticleSystem[] sporExplPool;
    

    void Start()
    {
        mossClusterArray = new List<GameObject>();
        mossVinesArray = new List<LineRenderer>();
        sporExplPool = new ParticleSystem[3];
        for (int i = 0; i < 3; i++)
        {
            sporExplPool[i] = Instantiate(sporeExplosion, transform.position, Quaternion.identity, transform);
            sporExplPool[i].gameObject.SetActive(false);
        }

        
        //clusters
        for (int i = 0; i < mossClusters; i++)
        {
            Vector3 direction = Random.insideUnitSphere;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, mossTryDistance))
            {
                GameObject mc = Instantiate(mossCluster, hit.point, Quaternion.identity, transform);
                mc.GetComponent<MossCluster>().Initialize(mossPlanesPerCluster, minMossRadius, maxMossRadius);
                mossClusterArray.Add(mc);
            }
        }
        //vines
        for (int i = 0; i < mossVinesAmount; i++)
        {
            int first = 0, second = 0;
            while (first == second)
            {
                first = Random.Range(0, mossClusterArray.Count);
                second = Random.Range(0, mossClusterArray.Count);
            }
            GameObject f = mossClusterArray[first], s = mossClusterArray[second];
            if(Vector3.Distance(f.transform.position, s.transform.position) < minVineDistance) continue;

            LineRenderer vine = Instantiate(mossVine, transform.position, quaternion.identity, transform).GetComponent<LineRenderer>();
            Vector3 middle = Vector3.Lerp(transform.position, (s.transform.position + f.transform.position) * 0.5f, Random.Range(0.6f, 0.8f));
            vine.SetPosition(4, middle);
            for (int j = 0; j < 9; j++)
            {
                if (j < 4)
                {
                    vine.SetPosition(j, Vector3.Lerp(f.transform.position, middle, j * 0.25f) + Random.insideUnitSphere * mossVinesVariance);
                }
                else if (j > 4)
                {
                    vine.SetPosition(j, Vector3.Lerp(middle, s.transform.position, j * 0.25f) + Random.insideUnitSphere * mossVinesVariance);
                }
            }
            mossVinesArray.Add(vine);
        }
        
        //rustfruits
        for (int i = 0; i < maxMossFruits * 0.5f; i++)
        {
            SpawnMossFruit(Random.Range(20,80));
        }
    }

    public void Update()
    {
        mossFruitTimer += Time.deltaTime;
        if(mossFruitTimer > mossFruitCooldown) SpawnMossFruit();
    }
    
    public void CreateSporeExplosion(Vector3 pos)
    {
        for (int i = 0; i < 3; i++)
        {
            if(sporExplPool[i].gameObject.activeInHierarchy)
                continue;
            
            sporExplPool[i].gameObject.SetActive(true);
            sporExplPool[i].transform.position = pos;
            sporExplPool[i].Play();
        }
    }

    public void SpawnMossFruit(float age = 0)
    {

        int randomVineIndex = Random.Range(0, mossVinesArray.Count);
        LineRenderer vine = mossVinesArray[randomVineIndex];
        Vector3 fruitPos = vine.GetPosition(Random.Range(4, 7));
        
        if(mossFruits >= maxMossFruits) return;
        
        Instantiate(mossFruit, fruitPos + Random.insideUnitSphere * 0.2f, Random.rotation, transform)
            .GetComponent<MossFruit>().Initialize(this, Random.value < chanceToDrop, age);
            
        mossFruits++;
        
        mossFruitTimer = 0;
    }

    public void MossFruitDied()
    {
        mossFruits--;
    }
    
}
