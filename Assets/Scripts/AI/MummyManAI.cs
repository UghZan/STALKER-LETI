using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class MummyManAI : MonoBehaviour
{
    private EnemyStats _stats;
    private AIMovement _movement;
    private Transform body;
    [SerializeField] private Animator _animator;

    public enum MummyState
    {
        LYING,
        STANDING,
        MOVING,
        ATTACK
    }

    public MummyState currentState;

    [Header("Feeling")]
    public float feelRadius = 1f;
    public float feelAggravatedRadius = 5f;
    
    [Header("Alarmness")]
    public float alarmnessLimit = 20f;
    [SerializeField] private float alarmness = 0f;
    public float alarmnessReduction = 1f;

    [Header("Attacking")] 
    public float attackDistance = 1f;

    private Vector3 lastFeltPos;
    private Vector3 defaultPos;
    private Player p;
    private float playerDist = 1000;

    private float attackCD;
    private void Start()
    {
        body = transform.GetChild(0);
        defaultPos = body.transform.localPosition;
        
        _stats = GetComponent<EnemyStats>();
        _movement = GetComponent<AIMovement>();
        //_animator = GetComponent<Animator>();

        currentState = Random.value > 0.5f ? MummyState.LYING : MummyState.STANDING;
        _animator.SetBool("lying", currentState == MummyState.LYING);
        transform.rotation = Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.up);

        p = FindObjectOfType<Player>();
        StartCoroutine(CheckForPlayer());
    }

    private void Update()
    {
        body.localPosition = currentState == MummyState.LYING ? Vector3.zero : defaultPos;
        _animator.SetFloat("speed", _movement.GetSpeed());
        playerDist = Vector3.Distance(transform.position, p.transform.position);
        
        switch (currentState)
        {
            case MummyState.LYING:
            case MummyState.STANDING:
                if(alarmness > alarmnessLimit)
                    SwitchToState(MummyState.MOVING);
                break;
            case MummyState.MOVING:
                if(alarmness <= 0)
                    SwitchToState(MummyState.STANDING);
                _movement.SetTarget(lastFeltPos);
                if(playerDist < attackDistance) SwitchToState(MummyState.ATTACK);
                break;
            case MummyState.ATTACK:
                if(playerDist > attackDistance && attackCD <= 0) SwitchToState(MummyState.MOVING);
                break;
        }

        if (_stats.isDead)
        {
            _stats.enabled = false;
            _movement.canMove = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            enabled = false;
            StopCoroutine(CheckForPlayer());
        }
        
        if (attackCD > 0) attackCD -= Time.deltaTime;
    }

    private void SwitchToState(MummyState state)
    {
        switch (state)
        {
            case MummyState.LYING:
                _animator.SetBool("lying", true);
                break;
            case MummyState.STANDING:
                _animator.SetBool("lying", false);
                break;
            case MummyState.MOVING:
                _animator.SetBool("lying", false);
                break;
            case MummyState.ATTACK:
                attackCD = 1.5f;
                _animator.SetTrigger("attack");
                _movement.SetTarget(transform.position);
                break;
        }

        currentState = state;
    }

    IEnumerator CheckForPlayer()
    {
        for(;;)
        {
            float speedMul = p.pc.GetSpeedMultiplier();
            if (currentState == MummyState.LYING || currentState == MummyState.STANDING)
            {
                if (playerDist < feelRadius)
                {
                    alarmness += speedMul * feelRadius/(playerDist + 0.01f);
                    lastFeltPos = p.transform.position;
                }
                else
                {
                    if (alarmness > 0) alarmness -= alarmnessReduction;
                }
            }
            else
            {
                if (playerDist < feelAggravatedRadius)
                    lastFeltPos = p.transform.position;
                else
                {
                    if (alarmness > 0) alarmness -= alarmnessReduction;
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
