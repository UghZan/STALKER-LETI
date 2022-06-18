using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Regen Buff")]
public class RegenBuff : Buff
{
    public float healthRegen;
    public float radRegen;
    public float bleedRegen;
    public float mentalRegen;
    public float staminaRegen;
    
    public override void OnTick(GenericStats stats)
    {
        stats.UpdateStat("health", healthRegen);
        stats.UpdateStat("rad", -radRegen);
        stats.UpdateStat("bleed", -bleedRegen);
        stats.UpdateStat("mental", mentalRegen);
        stats.UpdateStat("stamina", staminaRegen);
    }
}
