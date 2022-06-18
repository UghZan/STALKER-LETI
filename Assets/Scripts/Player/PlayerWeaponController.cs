using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject[] weapons;
    [Header("Aim Stats (readonly)")]
    public float aimDistance;
    public Vector3 aimPoint;
    [Header("References")]
    [SerializeField] Player owner;
    public bool weaponEquipped;
    bool rememberEquip;
    WeaponItem weapon;
    private void Start()
    {
        owner.invModule.OnWeaponEquipped.AddListener(UpdateEquippedWeapon);
    }

    void UpdateEquippedWeapon()
    {
        rememberEquip = weaponEquipped;

        weaponEquipped = false;
        if(rememberEquip)weapons[weapon.weaponIndex].SetActive(false);

        weapon = (WeaponItem)owner.invModule.equippedWeapon.GetItem().item;

        if(rememberEquip)EquipWeapon();
    }
    public void EquipWeapon()
    {
        if (!owner.invModule.equippedWeapon.equipped) return;

        weaponEquipped = !weaponEquipped;
        weapons[weapon.weaponIndex].SetActive(weaponEquipped);
        if(weaponEquipped)
        {
            weapons[weapon.weaponIndex].GetComponent<WeaponController>().InitStats(weapon);
        }
    }
}
