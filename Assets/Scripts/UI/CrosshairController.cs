using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] Image crosshair;
    [SerializeField] PlayerWeaponController pwc;

    [Header("Parameters")]
    public float maxScale;
    public float minScale;
    public float maxDistance;

    private float scaleValue;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }
    private void Update()
    {
        CheckScale();
        crosshair.rectTransform.localScale = new Vector2(scaleValue,scaleValue);
        crosshair.rectTransform.anchoredPosition = camera.WorldToScreenPoint(pwc.aimPoint);
    }

    private void CheckScale()
    {
        float val = Mathf.Clamp(pwc.aimDistance, 0, maxDistance) / maxDistance;
        scaleValue = HelperMethods.ProjectOnRange(val, 0, 1, minScale, maxScale);
    }
}
