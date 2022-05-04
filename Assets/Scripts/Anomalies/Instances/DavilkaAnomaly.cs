using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Anomalies.Instances
{
    public class DavilkaAnomaly : AnomalyBase
    {
        public Transform[] halfs;
        public float halfsRotSpeed = 1f;
        public List<Rigidbody> caughtObjects;
        public float pullForce = 1f;
        
        protected override void Update()
        {
            base.Update();
        
            halfs[0].Rotate(Vector3.up * (Time.deltaTime * halfsRotSpeed), Space.World);
            halfs[1].Rotate(Vector3.down * (Time.deltaTime * halfsRotSpeed), Space.World);
        }

        public void FixedUpdate()
        {
            for(int i=0; i < caughtObjects.Count; i++)
            {
                Rigidbody c = caughtObjects[i];
                try
                {
                    if (c.isKinematic) continue;
                    Vector3 dir = (transform.position - c.transform.position).normalized * pullForce;
                    c.AddForce(dir, ForceMode.Force);
                }
                catch
                {
                    caughtObjects[i] = null;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody comp;
            if(other.TryGetComponent<Rigidbody>(out comp)) caughtObjects.Add(comp);
            if (other.CompareTag("Bolt"))
                other.transform.localScale = new Vector3(other.transform.localScale.x, 0.5f, other.transform.localScale.z);
        }

        private void OnTriggerExit(Collider other)
        {
            Rigidbody comp;
            if(other.TryGetComponent<Rigidbody>(out comp))
                if (caughtObjects.Contains(comp))
                    caughtObjects.Remove(comp);
        }
    }
}
