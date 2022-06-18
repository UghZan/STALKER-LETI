using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Buff : ScriptableObject
{
    [HideInInspector]
    public GenericStats stats;
    [HideInInspector] 
    public BuffManager bm;
    public virtual void OnApply(GenericStats stats) {}
    public virtual void OnTick(GenericStats stats) {}
    public virtual void OnRemove(GenericStats stats) { }
    
    protected int ticks;
}
