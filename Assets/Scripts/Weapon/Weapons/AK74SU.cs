using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class AK74SU : WeaponController
{
    protected override void PrimaryFire()
    {
        float recoilX = Random.Range(-recoil, recoil);
        float recoilY = Random.Range(-recoil, recoil);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + recoilX, Screen.height / 2 + recoilY));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if(hit.rigidbody)
                if (hit.transform.TryGetComponent(out IDamageable dmg))
                {
                    dmg.TakeDamage(DamageType.Normal, -damage);
                    dmg.TakeDamage(DamageType.Bleed, damage * 0.1f);
                    hit.rigidbody.AddForceAtPosition(transform.forward * 10f, hit.point);
                    WeaponImpactController.instance.CreateImpactEffect(false, hit);
                    return;
                }
            WeaponImpactController.instance.CreateImpactEffect(true, hit);
        }
    
       // transform.GetComponent<Animator>().Play("ak_shoot");
    }

    protected override void SecondaryFire()
    {

    }
    

    protected override void Reload()
    {
        if(player.ak47ammo == 0 || reloadInProgress) return;
        currentAmmo = 0;
        animator.SetBool("reload", true);
    }
    
    public override void RefreshAmmo()
    {
        Debug.Log("ak47 refill");
        int ammoRefill = Mathf.Min(ammoMax, player.ak47ammo);
        player.ak47ammo -= ammoRefill;
        currentAmmo = ammoRefill;
    }

    protected override void AmmoCheck()
    {
        animator.SetBool("check", true);
    }
}
