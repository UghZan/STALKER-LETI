using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public WeaponItem weaponStats;

    [Header("Visual Parameters")]
    public ParticleSystem shoot;
    public bool reloadInProgress = false;
    public int currentAmmo { get; protected set; }

    [Header("Audio Parameters")] public AudioClip[] shootSounds;
    public AudioClip emptySound;
    public AudioClip reloadSound;

    [Header("Aim Parameters")]
    public Transform aimMuzzlePoint;
    public Vector3 aimEndPoint { get; protected set; }
    public float factualAimDistance { get; protected set; }

    [Header("References")]
    public Player owner;
    public Animator animator;
    public WeaponRecoilController wrc;
    public WeaponMovementController wmc;
    public AudioSource audioSource;

    protected float timer = 0f;
    private bool isLocked;

    protected abstract void PrimaryFire();
    protected abstract void SecondaryFire();
    protected abstract void Reload();

    public virtual void InitStats(WeaponItem item)
    {
        wrc.InitStats(weaponStats);
        wmc.InitStats(weaponStats);
        currentAmmo = weaponStats.ammoMax;
    }
    protected virtual void Start()
    {
        if (wmc == null) wmc = GetComponent<WeaponMovementController>();
        if (wrc == null) wrc = GetComponent<WeaponRecoilController>();
        wrc.InitStats(weaponStats);
        wmc.InitStats(weaponStats);
        currentAmmo = weaponStats.ammoMax;
        owner.uiModule.OnUIModeChanged.AddListener(UpdateLockMode);
    }
    void UpdateLockMode(bool lockMode)
    {
        isLocked = lockMode;
        wmc.isLocked = lockMode;
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1 / weaponStats.shootSpeed && currentAmmo > 0 && !isLocked)
        {
            if (weaponStats.isAutomatic)
            {
                if (Input.GetMouseButton(0))
                {
                    if (Random.value < weaponStats.muzzleChance) shoot.Play();

                    PrimaryFire();

                    wrc.Recoil();

                    timer = 0;

                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Random.value < weaponStats.muzzleChance) shoot.Play();

                    PrimaryFire();

                    wrc.Recoil();

                    timer = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        GetAimPoint();
    }

    protected void GetAimPoint()
    {
        Vector3 direction = transform.forward;
        Vector3 muzzlePoint = aimMuzzlePoint.position;
        if(Physics.Raycast(muzzlePoint, direction, out RaycastHit hit, weaponStats.range))
        {
            factualAimDistance = hit.distance;
            aimEndPoint = hit.point;
        }
        else
        {
            factualAimDistance = weaponStats.range;
            aimEndPoint = muzzlePoint + transform.forward * weaponStats.range;
        }
    }

    public virtual void RefreshAmmo()
    {
        currentAmmo = weaponStats.ammoMax;
    }
}
