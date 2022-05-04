using UnityEngine;

namespace Anomalies.Instances.Additional
{
    public class MossCluster : MonoBehaviour
    {
        public GameObject mossPlane;

        public void Initialize(int mossPlanes, float minRadius, float maxRadius)
        {
            for (int i = 0; i < mossPlanes; i++)
            {
                var point = i < (int)(mossPlanes * 0.25f) ? Random.insideUnitSphere * Random.Range(minRadius,maxRadius) : Vector3.zero;
                var distanceFromStart = Vector3.Distance(point, Vector3.zero);
                Instantiate(mossPlane, transform.position + point, Random.rotation, transform)
                    .transform.localScale *= Mathf.Clamp(((maxRadius-minRadius)/2)/(distanceFromStart + 0.01f), 0.25f, 1.5f);
            }
        }
    }
}