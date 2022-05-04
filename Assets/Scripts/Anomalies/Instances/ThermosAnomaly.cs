using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Anomalies.Instances
{
    public class ThermosAnomaly : AnomalyBase
    {
        public GameObject core;
        public ParticleSystem mainSystem;
        public ParticleSystem explosion;
        //public GameObject napalmObject;
        public bool disturbed;
        public float criticalCharge = 5f;
        public float napalmAmount = 16;
        public float napalmDistance = 8;

        public DamageZone fieldDamage;
        
        
        private float charge = 0f;

        private float tempSpeedMul;
        private ParticleSystem.MainModule mainSystemMain;

        private void Start()
        {
            mainSystemMain = mainSystem.main;
            tempSpeedMul = mainSystemMain.startSpeedMultiplier;
        }

        protected override void Update()
        {
            base.Update();
            if (disturbed || charge < 0)
                charge += Time.deltaTime;
            

            if (disturbed)
                mainSystemMain.startSpeedMultiplier = tempSpeedMul * charge * 2f;
            else
                mainSystemMain.startSpeedMultiplier = tempSpeedMul;


            if (charge < 0)
            {
                mainSystem.Stop();
                core.SetActive(false);
                fieldDamage.damage = 0.25f;
            }
            else
            {
                if(core.activeInHierarchy) core.SetActive(true);
                if (!mainSystem.isPlaying) mainSystem.Play();
                fieldDamage.damage = 1f;
            }


            if (charge >= criticalCharge)
            {
                explosion.Play();
                disturbed = false;
                charge = -25;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!disturbed && charge >= 0) {disturbed = true;}
        }
    }
}
