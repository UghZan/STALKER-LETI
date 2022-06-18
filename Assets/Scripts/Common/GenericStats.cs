using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericStats : MonoBehaviour, IDamageable
{
    [Header("Health")] 
    public float maxHealth = 100f;
    public float health;
    public float healthRegenCoefficient = 0.01f;
    [Space(5)] 
    public float rads = 0;
    public float healthLossPerRad = 0.02f;
    public float bleed = 0;
    public float healthLossPerBleed = 0.04f;
    [Header("Mental Health")] 
    public float maxMentalHealth = 100f;
    public float mentalHealth;
    public float mentalHealthRegenCoefficient = 0.01f;

    [Header("Resistances")] //multiplicative, means hm percent damage goes through
    public float normalVulnerability = 1f;
    public float electricVulnerability = 1f;
    public float hotVulnerability = 1f;
    public float freezeVulnerability = 1f;
    public float causticVulnerability = 1f;
    public float anomalVulnerability = 1f;
    public float radVulnerability = 1f;
    public float bleedVulnerability = 1f;
    public float mentalVulnerability = 1f;

    public float radDamageMultiplier = 1f;
    public float bleedDamageMultiplier = 1f;
    
    public bool isDead;

    public virtual void UpdateStat(string stat, float value)
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
            
        }
    }

    public virtual void UpdateMultiplier(string stat, float value)
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
            case "healthRegen":
                healthRegenCoefficient += value;
                break;
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
        mentalHealth = maxMentalHealth;
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Stress calculations
        if (health < maxHealth)
        {
            health += healthRegenCoefficient * Time.deltaTime;
        }

        health -= rads * healthLossPerRad * Time.deltaTime * radDamageMultiplier;
        health -= bleed * healthLossPerBleed * Time.deltaTime * (1 + rads/100) * bleedDamageMultiplier;

        if (health <= 0)
        {
            isDead = true;
        }
    }

    public void TakeDamage(DamageType type, float damage)
    {
        float scaledDamageNonInverted = HelperMethods.ProjectOnRange(damage, 0, 50, 0.15f, 1f);
        float scaledDamage = 1 - scaledDamageNonInverted;
        //if (type == DamageType.Normal || type == DamageType.Electric) rc.RecoilPunch(Mathf.Min(Mathf.Abs(damage), 16), Mathf.Min(Mathf.Abs(damage), 16), 1, Mathf.Abs(damage / health) + 0.1f, 0);
        switch (type)
        {
            case DamageType.Normal:
                DamageEffectManager.SetColor(new Color(1, scaledDamage, scaledDamage, 1));
                health = Mathf.Clamp(health + damage * normalVulnerability, 0, maxHealth);
                break;
            case DamageType.Electric:
                DamageEffectManager.SetContrastAndExposure(100 * scaledDamage, 4 * scaledDamage);
                health = Mathf.Clamp(health + damage * electricVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Freeze:
                DamageEffectManager.SetColor(new Color(scaledDamage, 1, 1, 1));
                health = Mathf.Clamp(health + damage * freezeVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Hot:
                DamageEffectManager.SetColor(new Color(1, scaledDamage, scaledDamage, 1));
                health = Mathf.Clamp(health + damage * hotVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Caustic:
                DamageEffectManager.SetColor(new Color(scaledDamage, 1f, scaledDamage, 1));
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
