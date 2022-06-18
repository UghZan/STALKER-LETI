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
    private BuffManager bm;
    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BuffManager _bm))
        {
            timer = timeUntilPoisoning;
            playerInside = true;
            bm = _bm;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BuffManager _bm))
        {
            timer = 0;
            playerInside = false;
            _bm = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        timer -= Time.deltaTime;
        if (playerInside && timer <= 0 && !bm.HasBuff(buff))
        {
            bm.AddBuff(new BuffEntry(buff, poisoningTime, bm));
        }
    }
    
    
}
