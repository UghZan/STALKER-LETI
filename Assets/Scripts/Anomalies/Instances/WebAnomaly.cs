using System;
using System.Collections;
using System.Collections.Generic;
using Anomalies;
using Common;
using UnityEngine;
using Random = UnityEngine.Random;

public class WebAnomaly : AnomalyBase
{
    [Header("Visual")]
    public int webTries;
    public float webRayDistance;
    public List<Vector3> webPoints;
    public GameObject webLineRenderer;

    [Header("Gameplay")] public float speedLimit;
    public float damage;
    public DamageType type;
    
    void Start()
    {
        for (int i = 0; i < webTries; i++)
        {
            Vector3 direction = Random.insideUnitSphere;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, webRayDistance))
            {
                webPoints.Add(hit.point);
            }
        }

        Vector3 oldPoint = Vector3.positiveInfinity;
        foreach (var p in webPoints)
        {
            if (oldPoint.Equals(Vector3.positiveInfinity))
            {
                oldPoint = p;
                continue;
            }
            GameObject webLine = Instantiate(webLineRenderer, transform.position, Quaternion.identity, transform);
            LineRenderer lr = webLine.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, oldPoint);
            lr.SetPosition(1, p);
            oldPoint = Vector3.positiveInfinity;
        }

        if (oldPoint != Vector3.positiveInfinity)
            webPoints.Remove(oldPoint);
    }

    protected override void OnObjectStay(Collider other)
    {
        if (other.TryGetComponent(out IMoving obj))
        {
            if(obj.movingSpeed > speedLimit)
            {
                if (other.TryGetComponent(out IDamageable dmg))
                {
                    dmg.TakeDamage(type, damage * (obj.movingSpeed - speedLimit));
                }
            }
        }
    }
}
