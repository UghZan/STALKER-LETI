using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapon.Weapons;

public class UIController : MonoBehaviour
{
    public Image magAmmoCounter;
    public Text magAmmoCounterText;

    public Image healthSignal;
    public Image radSignal;
    public Image bleedSignal;
    public Image mentalSignal;

    public Color extraLowHPColor;

    private PlayerStats p;
    [SerializeField] private AK74SU aksuRef;
    [SerializeField] private ASVAL asvalRef;
    private float timer = 0;
    private bool ammoType = false;

    private void Start()
    {
        aksuRef = FindObjectOfType<AK74SU>();
        asvalRef = FindObjectOfType<ASVAL>();
        p = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        UpdateSignals();
        if (timer > 0) timer -= Time.deltaTime;
    }

    private void UpdateSignals()
    {
        float healthCoeff = p.health / p.maxHealth;
        float rads = p.rads;
        float bleed = p.bleed;
        float mentalCoeff = p.mentalHealth / p.maxMentalHealth;

        
        if(timer > 0)
            magAmmoCounterText.text = ammoType ? asvalRef.currentAmmo + "/20" : aksuRef.currentAmmo + "/30";
        else
            magAmmoCounter.gameObject.SetActive(false);
        
        if (healthCoeff < 0.95)
            healthSignal.gameObject.SetActive(true);
        else
            healthSignal.gameObject.SetActive(false);

        if (healthCoeff > 0.66)
            healthSignal.color = Color.Lerp(Color.green, Color.yellow, 1 - (healthCoeff - 0.66f) / 0.29f);
        else if (healthCoeff > 0.33)
            healthSignal.color = Color.Lerp(Color.yellow, Color.red, 1 - (healthCoeff - 0.33f) / 0.33f);
        else
            healthSignal.color = Color.Lerp(Color.red, extraLowHPColor, 1 - healthCoeff / 0.33f);

        if (rads > 1)
            radSignal.gameObject.SetActive(true);
        else
            radSignal.gameObject.SetActive(false);

        if (rads > 50)
            radSignal.color = Color.red;
        else if (rads > 25)
            radSignal.color = Color.yellow;
        else
            radSignal.color = Color.green;

        if (bleed > 1)
            bleedSignal.gameObject.SetActive(true);
        else
            bleedSignal.gameObject.SetActive(false);

        if (bleed > 30)
            bleedSignal.color = Color.red;
        else if (bleed > 15)
            bleedSignal.color = Color.yellow;
        else
            bleedSignal.color = Color.green;
        
        if (mentalCoeff < 0.95)
            mentalSignal.gameObject.SetActive(true);
        else
            mentalSignal.gameObject.SetActive(false);

        if (mentalCoeff > 0.66)
            mentalSignal.color = Color.Lerp(Color.green, Color.yellow, 1 - (mentalCoeff - 0.66f) / 0.29f);
        else if (mentalCoeff > 0.33)
            mentalSignal.color = Color.Lerp(Color.yellow, Color.red, 1 - (mentalCoeff - 0.33f) / 0.33f);
        else
            mentalSignal.color = Color.Lerp(Color.red, extraLowHPColor, 1 - mentalCoeff / 0.33f);
    }

    public void ShowAmmoCounter(bool type)
    {
        ammoType = type;
        magAmmoCounter.gameObject.SetActive(true);
        timer = 3;
    }
}  
