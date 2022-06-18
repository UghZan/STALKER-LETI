using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffEntry
{
    public Buff buff;
    public int timer;

    public BuffEntry(Buff b, int t, BuffManager bm)
    {
        buff = b;
        buff.bm = bm;
        timer = t;
    }
}
