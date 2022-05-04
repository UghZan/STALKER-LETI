using System.Collections;
using System.Collections.Generic;
using Common;
using Inventory;
using UnityEngine;

public class ArtifactBase : ItemInfo
{
    [HideInInspector]
    public float artifactQuality = 1;
    public ArtifactTypes.ArtifactBirthType artifactBirthType;
    public ArtifactTypes.ArtifactType artifactType;

    protected ArtifactBeltController abc;
    protected Player plr;

    public virtual void OnEquip(ArtifactBeltController _abc, Player _p)
    {
        abc = _abc;
        plr = _p;
    }

    public virtual void OnUnequip()
    {
    }

    protected Buff TransformBuff(Buff b)
    {
        float posCoeff = GetPositiveCoefficient(), negCoeff = GetNegativeCoefficient();
        
        if (b is ResistBuff _b)
        {
            _b.normalResist *= _b.normalResist > 0 ? posCoeff : negCoeff;
            _b.anomalResist *= _b.anomalResist > 0 ? posCoeff : negCoeff;
            _b.electricResist *= _b.electricResist > 0 ? posCoeff : negCoeff;
            _b.hotResist *= _b.hotResist > 0 ? posCoeff : negCoeff;
            _b.freezeResist *= _b.freezeResist > 0 ? posCoeff : negCoeff;
            _b.radResist *= _b.radResist > 0 ? posCoeff : negCoeff;
            _b.bleedResist *= _b.bleedResist > 0 ? posCoeff : negCoeff;
            _b.mentalResist *= _b.mentalResist > 0 ? posCoeff : negCoeff;
            
            return _b;
        }
        else if (b is RegenBuff buff2)
        {
            buff2.healthRegen *= buff2.healthRegen > 0 ? posCoeff : negCoeff;
            buff2.radRegen *= buff2.radRegen > 0 ? posCoeff : negCoeff;
            buff2.bleedRegen *= buff2.bleedRegen > 0 ? posCoeff : negCoeff;
            buff2.mentalRegen *= buff2.mentalRegen > 0 ? posCoeff : negCoeff;
            buff2.staminaRegen *= buff2.staminaRegen > 0 ? posCoeff : negCoeff;
            
            return buff2;
        }
        else if (b is DamagingBuff buff3)
        {
            buff3.damagePower *= posCoeff;

            return buff3;
        }
        else if (b is MultiplierBuff buff4)
        {
            buff4.radDamage = buff4.radDamage > 0 ? posCoeff : negCoeff;
            buff4.bleedDamage = buff4.bleedDamage > 0 ? posCoeff : negCoeff;
            buff4.healthRegen *= buff4.healthRegen > 0 ? posCoeff : negCoeff;
            buff4.staminaRegen *= buff4.staminaRegen > 0 ? posCoeff : negCoeff;
            
            return buff4;
        }

        return null;
    }

    protected float GetPositiveCoefficient()
    {
        return 0.2513f * artifactQuality * artifactQuality + 0.7462f * artifactQuality + 0.0025f;
    }
    
    protected float GetNegativeCoefficient()
    {
        return -0.1168f * artifactQuality * artifactQuality - 0.6396f * artifactQuality + 1.7564f;
    }
}
