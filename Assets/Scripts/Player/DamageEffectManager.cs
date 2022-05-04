using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageEffectManager : MonoBehaviour
{
    public float recoverSpeed = 10f;
    private static Color _color;
    private static float _contrast;
    private static float _exposure;
    private Volume _volume;

    private ColorAdjustments _adjust;
    private PlayerStats ps;

    private void Start()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _adjust);
        ps = GetComponentInParent<PlayerStats>();
        _color = Color.white;
    }

    public static void SetColor(Color newColor)
    {
        _color = newColor;
    }
    
    public static void SetContrastAndExposure(float newContrast, float newExposure)
    {
        _contrast = newContrast;
        _exposure = newExposure;
    }

    public void Update()
    {
        _adjust.colorFilter.Override(_color);
        _adjust.contrast.Override(_contrast);
        _adjust.postExposure.Override(_exposure);
        
        _color = Color.Lerp(_color, Color.white, Time.deltaTime * recoverSpeed);
        _contrast = Mathf.Lerp(_contrast, 0, Time.deltaTime * recoverSpeed);
        _exposure = Mathf.Lerp(_exposure, 0, Time.deltaTime * recoverSpeed);
        _adjust.saturation.Override(MiscHelper.Map(ps.health, 0, 100, -66f,0f));
    }
}
