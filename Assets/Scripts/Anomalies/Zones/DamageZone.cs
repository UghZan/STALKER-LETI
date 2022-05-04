using System;
using Common;
using UnityEngine;

namespace Anomalies
{
    public class DamageZone : MonoBehaviour
    {
        public bool oneTime = false;
        public float damageInterval = 1;
        public float damage = 10f;
        public DamageType type = DamageType.Normal;

        private float timer;

        public void Update()
        {
            if (timer > 0) timer -= Time.deltaTime;
        }

        public void OnTriggerEnter(Collider other)
        {
            if(!oneTime) return;
            
            if(other.TryGetComponent(out IDamageable damaged)) damaged.TakeDamage(type, damage);
        }

        public void OnTriggerStay(Collider other)
        {
            if(oneTime) return;
            
            if(timer <= 0)
                if (other.TryGetComponent(out IDamageable damaged))
                {
                    damaged.TakeDamage(type, damage);
                    timer = damageInterval;
                }
        }
    }
}
