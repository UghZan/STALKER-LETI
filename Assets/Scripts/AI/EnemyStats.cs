using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class EnemyStats : GenericStats, IDamageable
{
    public void TakeDamage(DamageType type, float damage)
    {
        if(isDead) return;
        switch (type)
        {
            case DamageType.Normal:
                health = Mathf.Clamp(health + damage * normalVulnerability, 0, maxHealth);
                break;
            case DamageType.Electric:
                health = Mathf.Clamp(health + damage * electricVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Freeze:
                health = Mathf.Clamp(health + damage * freezeVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Hot:
                health = Mathf.Clamp(health + damage * hotVulnerability * anomalVulnerability, 0, maxHealth);
                break;
            case DamageType.Caustic:
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
