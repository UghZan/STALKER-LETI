using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class PlayerStats : GenericStats, IDamageable
{
    public RecoilController rc;
    private Player p;

    protected void Start()
    {
        base.Start();

        p = GetComponent<Player>();
        rc = GetComponentInChildren<RecoilController>();
    }
    
    public override void UpdateStat(string stat, float value)
    {
        switch (stat)
        {
            case "health":
                health = Mathf.Clamp(health + value, 0, maxHealth);
                break;
            case "rad":
                rads = Mathf.Clamp(rads + value, 0, 1000);
                break;
            case "bleed":
                bleed = Mathf.Clamp(bleed + value, 0, 1000);
                break;
            case "mental":
                mentalHealth = Mathf.Clamp(mentalHealth + value, 0, 1000);
                break;
            case "stamina":
                p.stamina = Mathf.Clamp(p.stamina + value, 0, p.maxStamina);
                break;
        }
    }
    
    public override void UpdateMultiplier(string stat, float value)
    {
        switch (stat)
        {
            case "normalVul":
                normalVulnerability += value;
                break;
            case "anomalVul":
                anomalVulnerability += value;
                break;
            case "electricVul":
                electricVulnerability += value;
                break;
            case "hotVul":
                hotVulnerability += value;
                break;
            case "freezeVul":
                freezeVulnerability += value;
                break;
            case "causticVul":
                causticVulnerability += value;
                break;
            case "radVul":
                radVulnerability += value;
                break;
            case "bleedVul":
                bleedVulnerability += value;
                break;
            case "mentalVul":
                mentalVulnerability += value;
                break;
            case "radDmgMul":
                radDamageMultiplier += value;
                break;
            case "bleedDmgMul":
                bleedDamageMultiplier += value;
                break;
            case "staminaRegen":
                p.staminaRegenCoefficient += value;
                break;
            case "healthRegen":
                healthRegenCoefficient += value;
                break;
        }
    }

    public void TakeDamage(DamageType type, float damage)
    {
        float scaledDamageNonInverted = MiscHelper.Map(damage, 0, 50, 0.15f, 1f);
        float scaledDamage = 1 - scaledDamageNonInverted;
        if(type == DamageType.Normal || type == DamageType.Electric) rc.RecoilPunch(Mathf.Min(Mathf.Abs(damage), 16),Mathf.Min(Mathf.Abs(damage), 16),1,Mathf.Abs(damage/health) + 0.1f, 0);
        switch (type) 
        {
            case DamageType.Normal:
                DamageEffectManager.SetColor(new Color(1,scaledDamage,scaledDamage, 1));
                health = Mathf.Clamp(health + damage * normalVulnerability, 0, maxHealth);
                break;
            case DamageType.Electric:
                DamageEffectManager.SetContrastAndExposure(100 * scaledDamage,4 * scaledDamage);
                health = Mathf.Clamp(health + damage * electricVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Freeze:
                DamageEffectManager.SetColor(new Color(scaledDamage,1,1, 1));
                health = Mathf.Clamp(health + damage * freezeVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Hot:
                DamageEffectManager.SetColor(new Color(1,scaledDamage,scaledDamage, 1));
                health = Mathf.Clamp(health + damage * hotVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Caustic:
                DamageEffectManager.SetColor(new Color(scaledDamage,1f,scaledDamage, 1));
                health = Mathf.Clamp(health + damage * causticVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Radioactive:
                rads = Mathf.Clamp(rads + damage * radVulnerability, 0, 1000);
                break;
            case DamageType.Bleed:
                bleed = Mathf.Clamp(bleed + damage * bleedVulnerability, 0, 1000);
                break;
            case DamageType.Mental:
                mentalHealth = Mathf.Clamp(mentalHealth + damage * mentalVulnerability, 0, maxMentalHealth);
                break;
        }
    }
}
