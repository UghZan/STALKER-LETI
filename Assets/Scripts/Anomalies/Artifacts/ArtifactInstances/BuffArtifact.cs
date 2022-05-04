using System.Collections;
using System.Collections.Generic;
using Common;
using Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Artifacts/Buffs Artifact")]
public class BuffArtifact : ArtifactBase
{
    public Buff[] appliedBuffs;

    private bool initialized = false;

    public override void OnEquip(ArtifactBeltController _abc, Player _p)
    {
        base.OnEquip(_abc, _p);

        if (!initialized)
        {
            for (int i = 0; i < appliedBuffs.Length; i++)
            {
                Buff tempBuff = TransformBuff(Instantiate(appliedBuffs[i]));
                tempBuff.plr = _p.s;
                appliedBuffs[i] = tempBuff;
            }

            initialized = true;
        }

        for (int i = 0; i < appliedBuffs.Length; i++)
        {
            abc.AddBuff(appliedBuffs[i]);
        }
    }

    public override void OnUnequip()
    {
        for (int i = 0; i < appliedBuffs.Length; i++)
        {
            abc.RemoveBuff(appliedBuffs[i]);
        }
    }
}