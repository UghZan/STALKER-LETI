using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    List<BuffEntry> buffs;
    [SerializeField] GenericStats stats;

    public void AddBuff(BuffEntry buff)
    {
        buffs.Add(buff);
        buff.buff.OnApply(stats);
    }

    public void Start()
    {
        buffs = new List<BuffEntry>();
        StartCoroutine(BuffTick());
    }

    IEnumerator BuffTick()
    {
        for (;;)
        {
            foreach (BuffEntry b in buffs.ToList())
            {
                if (b.timer > 0)
                {
                    b.buff.OnTick(stats);
                    b.timer--;
                }
            }

            buffs.RemoveAll(entry => entry.timer <= 0);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RemoveBuff(BuffEntry be)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i] == be)
            {
                buffs[i].buff.OnRemove(stats);
                buffs.RemoveAt(i);
            }
        }
    }
    
    public void RemoveBuff(Buff b)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].buff == b)
            {
                buffs[i].buff.OnRemove(stats);
                buffs.RemoveAt(i);
            }
        }
    }

    public bool HasBuff(Buff b)
    {
         return buffs.Exists(entry => entry.buff == b);
    }
}
