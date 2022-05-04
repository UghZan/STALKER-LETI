using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private Vector3 target;

    public bool canMove = true;
    public float movementSpeed = 5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
    }

    private void Update()
    {
        agent.speed = movementSpeed;
        agent.SetDestination(canMove ? target : transform.position);

        agent.enabled = canMove;
    }

    public void SetTarget(Vector3 _target)
    {
        target = _target;
    }

    public float GetSpeed()
    {
        return agent.velocity.magnitude;
    }
}
