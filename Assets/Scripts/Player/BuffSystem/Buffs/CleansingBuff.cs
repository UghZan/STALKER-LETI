using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Cleansing Buff")]
public class CleansingBuff : Buff
{
    public Buff[] buffsToRemove;


    public override void OnApply()
    {
        foreach (var buff in buffsToRemove)
        {
            bm.RemoveBuff(buff);
        }
    }
}
