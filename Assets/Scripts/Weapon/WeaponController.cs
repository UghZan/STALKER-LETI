using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WeaponController : MonoBehaviour
{
    [Header("Gameplay Vars")]
    public float damage;
    public float range;
    public int ammoMax;
    public bool isAutomatic;
    public float shootSpeed;
    
    [Header("Recoil Vars")]
    public bool fixedRecoil;
    public float inaccuracyAmount;
    public float horizontalCamRecoil = 3;
    public float verticalCamRecoil = 8;
    public float negativeVerticalRecoilReduction = -0.2f;

    [Header("Recoil Increase Vars")]
    public float recoilIncreaseStep = 0.1f;
    public float maxRecoilIncrease = 1;
    public float minRecoil = 0.25f;
    private float recoilIncrease;

    [Header("Visual Vars")]
    public ParticleSystem shoot;
    public float muzzleChance = 0.66f;
    public bool reloadInProgress = false;
    protected float recoil;
    public int currentAmmo { get; protected set; }

    [Header("Audio Vars")] public AudioClip[] shootSounds;
    public AudioClip emptySound;
    public AudioClip reloadSound;
    
    [Header("References")]
    public Animator animator;
    public Player player;
    public AudioSource audioSource;
    
    protected float timer = 0f;
    public WeaponMovementController wmc;
    public RecoilController rc;

    protected abstract void PrimaryFire();
    protected abstract void SecondaryFire();

    protected abstract void Reload();
    protected abstract void AmmoCheck();

    private void Start()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
        if(!wmc)
            wmc = GetComponent<WeaponMovementController>();
        currentAmmo = ammoMax;

    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1 / shootSpeed && currentAmmo > 0 && !player.invOpen && !reloadInProgress)
        {
            if (isAutomatic)
            {
                if(Input.GetMouseButton(0))
                {
                    if(Random.value < muzzleChance) shoot.Play();
                    PrimaryFire();
                    wmc.Punch(recoilIncrease);
                    rc.RecoilPunch(horizontalCamRecoil, verticalCamRecoil, negativeVerticalRecoilReduction, recoilIncrease, minRecoil);
                    timer = 0;
                    if (!fixedRecoil)
                        recoil = Mathf.Lerp(recoil, inaccuracyAmount, recoilIncreaseStep);
                    recoilIncrease = Mathf.Clamp(recoilIncrease + recoilIncreaseStep, 0, maxRecoilIncrease);
                    currentAmmo--;

                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(Random.value < muzzleChance) shoot.Play();
                    PrimaryFire();
                    rc.RecoilPunch(horizontalCamRecoil, verticalCamRecoil, negativeVerticalRecoilReduction, recoilIncrease, minRecoil);
                    wmc.Punch(recoilIncrease);
                    timer = 0;
                    currentAmmo--;
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            recoil = 0;
            recoilIncrease = 0;
        }

        if (Input.GetKeyDown(KeyCode.R) )
        {
            Reload();
        }
        
        if (Input.GetKeyDown(KeyCode.Z) )
        {
            AmmoCheck();
        }

    }

    public virtual void RefreshAmmo()
    {
        currentAmmo = ammoMax;
    }
}
