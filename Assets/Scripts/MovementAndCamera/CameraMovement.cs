using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject playerBody;
    private Camera cum;

    public float normalFOV = 75;
    public float fovChangeSpeed = 2f;

    [Header("Tilting")]
    public float tiltCoefficient = 15f;
    public float tiltSpeed = 2f;

    [Header("Bobbing")]
    public float bobSpeed = 4f;
    public float bobAmount = 0.5f;
    public float bobMul = 2f;

    public Vector3 tiltDirection;

    [Header("Sprintng Multiplier")]
    public float sprintBobSpeedMultiplier = 1.5f;
    public float sprintBobAmountMultiplier = 1.5f;
    public float sprintFOV = 90;
    
    [Header("Slow-Walking Multiplier")]
    public float slowBobSpeedMultiplier = 0.5f;
    public float slowBobAmountMultiplier = 0.5f;
    public float slowFOV = 60;

    public bool isSprinting;
    public bool isSlowWalking;

    PlayerController pc;
    [SerializeField] private GameObject head;

    Vector3 startPosition;
    Quaternion startRotation;
    float speed = 0;
    float fov;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        pc = playerBody.GetComponent<PlayerController>();
        cum = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tiltVector = Quaternion.AngleAxis(90, Vector3.up) *
                             (Vector3.up + tiltDirection.normalized * tiltCoefficient);
        Quaternion tiltRot = Quaternion.Euler(tiltVector);
        head.transform.localRotation =
            Quaternion.Slerp(head.transform.localRotation, tiltRot, Time.deltaTime * tiltSpeed);

        speed = tiltDirection.normalized.magnitude;
        FOVControl();
        CameraBob();
    }
    

    void FOVControl()
    {
        if (isSprinting)
            fov = Mathf.Lerp(fov, sprintFOV, Time.deltaTime * fovChangeSpeed);
        else
            if(isSlowWalking)
                fov = Mathf.Lerp(fov, slowFOV, Time.deltaTime * fovChangeSpeed);
            else
                fov = Mathf.Lerp(fov, normalFOV, Time.deltaTime * fovChangeSpeed);
        cum.fieldOfView = fov;
    }

    void CameraBob()
    {
        float speedMul, strMul;
        if (isSprinting)
        {
            speedMul = sprintBobSpeedMultiplier;
            strMul = sprintBobAmountMultiplier;
        }
        else
        {
            if (isSlowWalking)
            {
                speedMul = slowBobSpeedMultiplier;
                strMul = slowBobAmountMultiplier;
            }
            else
            {
                strMul = 1;
                speedMul = 1;
            }
        }
        float y = bobAmount * strMul * speed * Mathf.Sin(2 * bobSpeed * speedMul * Time.time);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(startPosition.x, startPosition.y + y, startPosition.z), Time.deltaTime * bobMul);
    }

}
