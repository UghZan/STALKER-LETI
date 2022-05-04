using System;
using UnityEngine;
using System.Collections.Generic;

namespace Anomalies.Instances
{
    public class FireflyAnomaly : AnomalyBase
    {
        public Transform[] trackedObjects;
        public ParticleSystem[] ps;

        public void Start()
        {
            trackedObjects = new Transform[ps.Length];
        }

        private void OnTriggerEnter(Collider other)
        {
            bool all = true;
            for (int i = 0; i < ps.Length; i++)
            {
                if (trackedObjects[i] == null)
                {
                    trackedObjects[i] = other.transform; 
                    ps[i].Play(); 
                    ps[i].transform.SetParent(other.transform);
                    ps[i].transform.localPosition = Vector3.zero;
                    all = false;
                    break;
                }
            }

            if (all)
            {
                trackedObjects[0] = other.transform;
                ps[0].transform.SetParent(other.transform);
                ps[0].transform.localPosition = Vector3.zero;
                ps[0].Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            for (int i = 0; i < ps.Length; i++)
            {
                if (trackedObjects[i] == other.transform)
                {
                    trackedObjects[i] = null; 
                    ps[i].transform.SetParent(transform); 
                    ps[i].Stop(); 
                    return;
                }
            }
        }
    }
}
