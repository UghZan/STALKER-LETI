using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public interface IDamageable
{
    //0 - default
    //1 - rad
    //2 - bleed
    //3 - mental
    public void TakeDamage(DamageType type, float damage);
}
