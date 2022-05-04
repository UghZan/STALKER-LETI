using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffEntry
{
    public Buff buff;
    public int timer;

    public BuffEntry(PlayerStats p, Buff b, int t, BuffManager bm)
    {
        buff = b;
        buff.plr = p;
        buff.bm = bm;
        timer = t;
    }
}
