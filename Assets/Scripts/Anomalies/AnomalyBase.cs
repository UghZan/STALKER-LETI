using Anomalies.Generation;
using UnityEngine;

namespace Anomalies
{
    public class AnomalyBase : MonoBehaviour
    {
        public float entryCooldown = 1.0f;
        public float anomalyPower = 1.0f;
        private float cooldown;

        private ArtifactGeneration genModule;

        protected virtual void Start()
        {
            genModule = GetComponent<ArtifactGeneration>();
            var damageZones = GetComponentsInChildren<DamageZone>();
            foreach (var dz in damageZones)
            {
                dz.damage *= anomalyPower;
            }
        }
        
        protected virtual void OnObjectStay(Collider other)
        {
        }

        protected virtual void Update()
        {
            if (cooldown > 0)
                cooldown -= Time.deltaTime;
        }

        public void OnTriggerStay(Collider other)
        {
            if (cooldown <= 0)
            {
                OnObjectStay(other);
                cooldown = entryCooldown;
            }
        }
    }
}
