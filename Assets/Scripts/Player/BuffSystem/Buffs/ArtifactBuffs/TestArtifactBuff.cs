using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Buffs/Artifact/Test Artifact Buff")]
public class TestArtifactBuff: Buff
{
    public float bulletProt;
    public float healthRegen;

    public TestArtifactBuff(float _bP, float _hR)
    {
        bulletProt = _bP;
        healthRegen = _hR;
    }

    public override void OnApply()
    {
        plr.UpdateMultiplier("normal", bulletProt);
    }

    public override void OnTick()
    {
        plr.UpdateStat("health", healthRegen);
    }

    public override void OnRemove()
    {
        plr.UpdateMultiplier("normal", -bulletProt);
    }
}