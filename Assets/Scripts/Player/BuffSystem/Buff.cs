using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Buff : ScriptableObject
{
    [HideInInspector]
    public PlayerStats plr;
    [HideInInspector] 
    public BuffManager bm;
    public virtual void OnApply() {}
    public virtual void OnTick() {}
    public virtual void OnRemove() {}
    
    protected int ticks;
}
