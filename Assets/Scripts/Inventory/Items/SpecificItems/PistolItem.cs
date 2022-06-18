using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/New Pistol Instance")]
public class PistolItem : WeaponItem
{
    [Header("Pistol Specific")]
    public float timeUntilRecharge;
    public float rechargeRate;
}
