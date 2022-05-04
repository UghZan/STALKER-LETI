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
    
    public override void OnTick()
    {
        plr.UpdateStat("health", healthRegen);
        plr.UpdateStat("rad", -radRegen);
        plr.UpdateStat("bleed", -bleedRegen);
        plr.UpdateStat("mental", mentalRegen);
        plr.UpdateStat("stamina", staminaRegen);
    }
}
