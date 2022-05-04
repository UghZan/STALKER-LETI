using System.Timers;
using Inventory;
using UnityEngine;

namespace Anomalies.Generation
{
    public class ArtifactGeneration : MonoBehaviour
    {
        [Header("Spawn Settings")]
        public GameObjectSpawnList artifacts;
        public float artifactSpawnChance = 0.1f;
        [Header("Position Settings")] 
        public float exclusionDistance = 1f;
        public float spawnTryDistance = 5f;
        public int spawnTries = 2;
        [Header("Gameplay Settings")]
        public float averageArtifactQuality = 0.75f;
        public float artifactGenerationCooldown = 600;

        private float timer;
        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= artifactGenerationCooldown)
            {
                if (Random.value < artifactSpawnChance)
                    GenerateArtifact();
                timer = 0;
            }
        }

        public GameObject GenerateArtifact()
        {
            GameObject art = artifacts.GetEntry();
            var artBase = art.GetComponent<ItemPickup>().keptItem as ArtifactBase;
            var newArt = Instantiate(artBase);
            newArt.artifactQuality = GaussianRandom.NextGaussian(averageArtifactQuality, 1f, 0f, 2f);
            GameObject finalArt = Instantiate(art, GetRandomPositionAround(), Random.rotation);
            finalArt.GetComponent<ItemPickup>().keptItem = newArt;
            return finalArt;
        }

        public Vector3 GetRandomPositionAround()
        {
            Vector3 direction = (Random.insideUnitSphere + Vector3.down * 2).normalized;
            for (int i = 0; i < spawnTries; i++)
            {
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, spawnTryDistance))
                {
                    if (Vector3.Distance(hit.point, transform.position) > exclusionDistance)
                        return hit.point;
                }
            }

            return transform.position;
        }
    }
}