using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponMovementController : MonoBehaviour
{
    [Header("Sway Settings")]
    public float maxSway;
    public float swayFactor;
    public float swayRotateFactor;
    public float swayDamp;
    public float swayRotateDamp;

    [Header("Bob Settings")]
    public float bobAmount;
    public float bobSpeed;

    [Header("Breath Settings")]
    public float breathAmount;
    public float breathSpeed;

    [Header("Aimpunch Settings")]
    public float horizontalRotPunch = 0.2f;
    public float verticalRotPunch = 0.1f;
    public float pushBackForce = 1f;
    //public float maxPushBack = -2f;
    public float negativeVerticalReduction = -0.2f;
    public float returnSpeed = 5f;
    public float changeSpeed = 10f;

    Vector3 position;
    private Vector3 rotationRecoil;
    private Vector3 positionRecoil;
    private Vector3 rotDelta;
    
    // Use this for initialization
    void Start()
    {
        position = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, Time.deltaTime * returnSpeed);
        positionRecoil = Vector3.Lerp(positionRecoil, Vector3.zero, Time.deltaTime * returnSpeed);
        
        rotDelta = Vector3.Slerp(rotDelta, rotationRecoil, Time.deltaTime * changeSpeed);
        transform.localRotation = Quaternion.Euler(rotDelta);
        transform.localPosition += positionRecoil;

        Sway();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            Walk();
        else
            Breath();
        
    }

    public void Sway()
    {
        float fX = Input.GetAxisRaw("Mouse X") * swayFactor;
        float fY = Input.GetAxisRaw("Mouse Y") * swayFactor;

        fX = Mathf.Clamp(fX, -maxSway, maxSway);
        fY = Mathf.Clamp(fY, -maxSway, maxSway);

        Vector3 final = new Vector3(position.x - fX, position.y - fY, position.z);
        Vector3 rotate = Vector3.up * (fX * swayRotateFactor);
        transform.localPosition = Vector3.Lerp(transform.localPosition, final, swayDamp);
        //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotate), swayRotateDamp);
    }

    public void Walk()
    {
        float xBob = 2 * Mathf.Sin(Time.time * bobSpeed + Mathf.PI / 2) * bobAmount;
        float yBob = Mathf.Sin(Time.time * 2 * bobSpeed) * bobAmount;

        Vector3 final = new Vector3(position.x + xBob, position.y + yBob, position.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, final, 0.1f*Time.deltaTime);
    }

    public void Breath()
    {
        float yBob = Mathf.Sin(Time.time * 2 * breathSpeed) * breathAmount;

        Vector3 final = new Vector3(position.x, position.y + yBob, position.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, final, 0.1f*Time.deltaTime);
    }

    public void Punch(float coef)
    {
        float fX = Random.Range(-horizontalRotPunch, horizontalRotPunch);
        float fY = Random.Range(-verticalRotPunch * negativeVerticalReduction, verticalRotPunch);
        
        Vector3 rotate = Vector3.up * fX + Vector3.left * fY;
        Vector3 pushBack = Vector3.back * pushBackForce;
        
        rotationRecoil += rotate * (Mathf.Max(coef, 0.25f));
        positionRecoil = pushBack * (Mathf.Max(coef, 0.25f));
    }
}
