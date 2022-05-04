using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreewindAnomaly : MonoBehaviour
{
    public Vector3[] positions;
    public float moveSpeed;
    public float distanceThreshold;

    private int nextWaypoint = 1;
    private Vector3 currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = positions[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, currentTarget) < distanceThreshold)
        {
            currentTarget = positions[nextWaypoint];
            if (nextWaypoint + 1 >= positions.Length)
                nextWaypoint = 0;
            else
                nextWaypoint++;
        }
    }
}
