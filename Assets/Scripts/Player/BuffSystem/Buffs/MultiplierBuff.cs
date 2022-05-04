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

    public override void OnApply()
    {
        plr.UpdateMultiplier("radDmgMul", -radDamage);
        plr.UpdateMultiplier("bleedDmgVul", -bleedDamage);
        plr.UpdateMultiplier("healthRegen", healthRegen);
        plr.UpdateMultiplier("staminaRegen", staminaRegen);
    }

    public override void OnRemove()
    {
        plr.UpdateMultiplier("radDmgMul", radDamage);
        plr.UpdateMultiplier("bleedDmgVul", bleedDamage);
        plr.UpdateMultiplier("healthRegen", -healthRegen);
        plr.UpdateMultiplier("staminaRegen", -staminaRegen);
    }
}