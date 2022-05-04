using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RecoilController : MonoBehaviour
{
    public Quaternion startRotation;

    [Header("Aimpunch Settings")]
    public float returnSpeed = 5f;
    public float changeSpeed = 10f;
    
    Vector3 position;
    private Vector3 currentRotation;
    private Vector3 rotDelta;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, Time.deltaTime * returnSpeed);
        rotDelta = Vector3.Slerp(rotDelta, currentRotation, Time.deltaTime * changeSpeed);
        transform.localRotation = Quaternion.Euler(rotDelta);
    }

    public void RecoilPunch(float r_X, float r_Y, float neg, float coef, float minCoeff)
    {
        float fX = Random.Range(-r_X, r_X);
        float fY = Random.Range(-r_Y * neg, r_Y);
        
        Vector3 rotate = Vector3.up * fX + Vector3.left * fY;
        
        currentRotation += rotate * (Mathf.Max(coef, minCoeff));
        
    }
}
