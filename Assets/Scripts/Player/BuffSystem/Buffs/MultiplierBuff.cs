using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Multiplier Buff")] 
public class MultiplierBuff : Buff
{
    public float staminaRegen;
    public float healthRegen;
    public float bleedDamage;
    public float radDamage;

    public override void OnApply(GenericStats stats)
    {
        stats.UpdateMultiplier("radDmgMul", -radDamage);
        stats.UpdateMultiplier("bleedDmgVul", -bleedDamage);
        stats.UpdateMultiplier("healthRegen", healthRegen);
        stats.UpdateMultiplier("staminaRegen", staminaRegen);
    }

    public override void OnRemove(GenericStats stats)
    {
        stats.UpdateMultiplier("radDmgMul", radDamage);
        stats.UpdateMultiplier("bleedDmgVul", bleedDamage);
        stats.UpdateMultiplier("healthRegen", -healthRegen);
        stats.UpdateMultiplier("staminaRegen", -staminaRegen);
    }
}