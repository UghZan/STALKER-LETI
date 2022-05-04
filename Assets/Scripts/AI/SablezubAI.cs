using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SablezubAI : MonoBehaviour
{
    enum SablezubState
    {
        IDLE,
        ALERT,
        POUNCE,
        MELEE
    }
    
    private EnemyStats _stats;
    private AIMovement _mov;

    [Header("Pounce")]
    public float pounceDistance;
    public float pounceCooldown;
    public float pounceForce;
    public GameObject pounceZone;
    
    [Header("Common")]
    public float groundCheckDistance = 0.6f;
    public float fov = 90;
    public float visionDistance = 10f;

    [Header("Melee")] 
    public float meleeDistance = 3f;

    [SerializeField] private SablezubState currentState;

    private float alarmness;
    private float cooldown;
    private float jumpCooldown;
    private Vector3 pounceTarget;
    private Vector3 target;

    private Player p;
    private Rigidbody r;

    private void Start()
    {
        cooldown = pounceCooldown;
        _stats = GetComponent<EnemyStats>();
        _mov = GetComponent<AIMovement>();
        p = FindObjectOfType<Player>();
        
        r = GetComponent<Rigidbody>();

        StartCoroutine(CheckForPlayer());
    }

    private void Update()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;
        switch (currentState)
        {
            case SablezubState.POUNCE:
                if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance) && jumpCooldown <= 0)
                {
                    r.velocity = Vector3.zero;
                    _mov.canMove = true;
                    pounceZone.SetActive(false);
                    SwitchStates(SablezubState.ALERT);
                }
                break;
            case SablezubState.IDLE:
                break;
            case SablezubState.ALERT:
                if (cooldown <= 0 && Vector3.Distance(transform.position, target) < pounceDistance && Vector3.Distance(transform.position, target) > meleeDistance)
                    SwitchStates(SablezubState.POUNCE);
                else
                    _mov.SetTarget(target);

                if (Vector3.Distance(transform.position, target) < meleeDistance)
                {
                    SwitchStates(SablezubState.MELEE);
                }
                break;
            case SablezubState.MELEE:
                _mov.SetTarget(p.transform.position);
                if (Vector3.Distance(transform.position, target) > meleeDistance)
                {
                    SwitchStates(SablezubState.ALERT);
                }
                break;
        }
        
    }

    private void SwitchStates(SablezubState state)
    {
        switch (state)
        {
            case SablezubState.IDLE:
                break;
            case SablezubState.ALERT:
                break;
            case SablezubState.MELEE:
                break;
            case SablezubState.POUNCE:
                Jump();
                break;
        }

        currentState = state;
    }

    private void Jump()
    {
        pounceTarget = p.transform.position;
        pounceZone.SetActive(true);
        
        _mov.canMove = false;
        jumpCooldown = 2f;
        
        r.velocity = (pounceTarget-transform.position) + Vector3.up * Mathf.Sqrt(2f * -Physics.gravity.y * pounceForce);
        
        cooldown = pounceCooldown;

    }
    
    IEnumerator CheckForPlayer()
    {
        for(;;)
        {
            Vector3 dir = p.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(dir.normalized, Vector3.up)) < fov / 2)
            {
                Debug.DrawRay(transform.position, dir.normalized * visionDistance);
                if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, visionDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        alarmness += 0.25f;
                    }


                    if (alarmness > 3f)
                    {
                        if(currentState == SablezubState.IDLE)
                            SwitchStates(SablezubState.ALERT);
                        target = p.transform.position;
                    }
                }
                else
                {
                    alarmness -= 0.33f;
                }
            }
            else
            {
                alarmness -= 0.33f;
            }
            
            if(alarmness <= 0)
                SwitchStates(SablezubState.IDLE);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
