using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolWeapon : WeaponController
{
    [Header("Pistol Specific Stats")]
    public GameObject projectile;
    public float projSpeed;
    private PistolItem pistolStats;

    private float chargeTimer;


    protected override void Start()
    {
        base.Start();
        pistolStats = (PistolItem)weaponStats;    
    }

    protected override void PrimaryFire()
    {
        float spread = weaponStats.weaponSpread;
        Vector3 inaccuracy = new Vector2(Random.Range(-spread, +spread), Random.Range(-spread, +spread));
        GameObject p = Instantiate(projectile, aimMuzzlePoint.position, Quaternion.LookRotation(transform.forward + inaccuracy));
        p.GetComponent<PistolProjectile>().speed = projSpeed;
        currentAmmo -= pistolStats.ammoPerShot;
    }

    protected override void Update()
    {
        base.Update();

        if(timer > pistolStats.timeUntilRecharge)
        {
            chargeTimer += Time.deltaTime * pistolStats.rechargeRate;
            if (chargeTimer > 1 && currentAmmo < pistolStats.ammoMax) { currentAmmo++; chargeTimer = 0; }
        }
    }

    protected override void Reload()
    {
        RefreshAmmo();
    }

    protected override void SecondaryFire()
    {
        
    }
}
