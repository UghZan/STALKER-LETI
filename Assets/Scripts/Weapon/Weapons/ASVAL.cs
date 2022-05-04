using Common;
using UnityEngine;

namespace Weapon.Weapons
{
    public class ASVAL : WeaponController
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
            if(player.asvalammo == 0 || reloadInProgress) return;
            animator.SetBool("reload", true);
        }
    
        public override void RefreshAmmo()
        {
            Debug.Log("asval refill");
            int ammoRefill = Mathf.Min(ammoMax, player.asvalammo);
            player.asvalammo -= ammoRefill;
            currentAmmo = ammoRefill;
        }

        protected override void AmmoCheck()
        {
            animator.SetBool("check", true);
        }
    }
}