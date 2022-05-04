using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ElmGasAnomaly : MonoBehaviour
{
    public float timeUntilPoisoning;
    public Buff buff;
    public int poisoningTime;

    private bool playerInside;
    private Player p;
    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _bm))
        {
            timer = timeUntilPoisoning;
            playerInside = true;
            p = _bm;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _bm))
        {
            timer = 0;
            playerInside = false;
            p = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        timer-=Time.deltaTime;
        if (playerInside && timer <= 0 && !p.CheckBuff(buff))
        {
            p.AddBuff(new BuffEntry(p.s, buff, poisoningTime, p.buffM));
        }
    }
    
    
}
