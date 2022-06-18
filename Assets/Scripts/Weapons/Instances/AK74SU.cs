using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK74SU : WeaponController
{
    protected override void Start()
    {
        base.Start(); 
    }

    protected override void PrimaryFire()
    {
        animator.SetTrigger("cock");
        float recoilX = Random.Range(-weaponStats.weaponSpread, weaponStats.weaponSpread);
        float recoilY = Random.Range(-weaponStats.weaponSpread, weaponStats.weaponSpread);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + recoilX, Screen.height / 2 + recoilY));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, weaponStats.range))
        {
            if (hit.rigidbody)
                if (hit.transform.TryGetComponent(out IDamageable dmg))
                {
                    //dmg.TakeDamage(DamageType.Normal, -damage);
                    //dmg.TakeDamage(DamageType.Bleed, damage * 0.1f);
                    hit.rigidbody.AddForceAtPosition(transform.forward * 10f, hit.point);
                    return;
                }
        }
        currentAmmo--;
    }

    protected override void Reload()
    {
        if (reloadInProgress) return;
        currentAmmo = 0;
        animator.SetBool("reload", true);
    }

    protected override void SecondaryFire()
    {
        
    }
}
