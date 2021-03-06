using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Damaging Buff")]
public class DamagingBuff : Buff
{
    public DamageType damageType;
    public float damagePower;
    public int ticksToDamage;
    public override void OnTick()
    {
        ticks++;
        if(ticks%ticksToDamage==0) plr.TakeDamage(damageType, damagePower);
    }
}
