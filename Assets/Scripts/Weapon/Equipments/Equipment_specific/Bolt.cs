using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Weapon.Equipments;

public class Bolt : GrenadeBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void SecondaryFire()
    {
        manager.StartCoroutine("ThrowAnimation");
        Transform cam = manager.player.cam.transform;
        GameObject _grenade = Instantiate(grenadePrefab, cam.position + cam.forward * 0.25f, Quaternion.identity);
        _grenade.GetComponent<Rigidbody>().AddForce(cam.forward * manager.throwPower, ForceMode.VelocityChange);
        _grenade.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere);
        Destroy(_grenade, 60);
        //isExpired = true;
    }
}
