using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMoving
{

    //Character Controller
    //Partially adapted from FPS Microgame's PlayerCharacterController.cs

    [Header("References")]
    public GameObject playerCamera;
    public GameObject playerLegs;

    [Header("Movement")]
    public float rotationMultiplier = 10f;
    public float maxMovementSpeed = 10f;
    public float sprintMultiplier = 2f;
    public float slowWalkMultiplier = 0.5f;
    public float acceleration = 10f;
    public float speedMultiplier = 1f;

    [Header("Jumping")]
    public float groundCheckDistance = 0.1f;
    public float jumpForce = 2f;
    public float additionalGravityForce = 10f;

    [Header("Misc")] 
    public Vector3 aimPos;
    public bool isSprinting;
    public bool isOnGround;
    public bool isMoving;
    public bool isSlowWalking;
    public float speed;
    public bool canMove = true;

    PlayerInputHandler inputHandler;
    Rigidbody rb;
    CapsuleCollider col;
    CameraMovement camMov;
    private Player _player;
    
    float m_cameraAngle;
    Vector3 currentVelocity;
    bool hasJumpedInThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        externalSpeedMultiplier = 1f;
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        camMov = GetComponentInChildren<CameraMovement>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
            GroundCheck();

            HandleMovement();

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                isSlowWalking = true;
                isSprinting = false;
            }
            else
            {
                isSlowWalking = false;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && _player.stamina > 10f && !isSlowWalking)
            {
                isSprinting = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || _player.stamina <= 0)
            {
                isSprinting = false;
            }
            camMov.isSprinting = isSprinting;
            camMov.isSlowWalking = isSlowWalking;
    }

    public void ControlMouseLock(bool state) // true - invMode, false - default
    {
        inputHandler.mouseLock = state;
        inputHandler.SetCursorState(state ? CursorLockMode.Confined : CursorLockMode.Locked, state);
    }
    
    private void GroundCheck()
    {
        isOnGround = false;
        Ray ray = new Ray(transform.TransformPoint(col.center), Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, -1, QueryTriggerInteraction.Ignore))
        {
            isOnGround = true;
        }
    }

    void HandleMovement()
    {

        transform.Rotate(new Vector3(0, inputHandler.GetMouseHorizontal(), 0), Space.Self);

        m_cameraAngle += inputHandler.GetMouseVertical();
        m_cameraAngle = Mathf.Clamp(m_cameraAngle, -90, 90);
        playerCamera.transform.localEulerAngles = new Vector3(-m_cameraAngle, 0, 0);

        aimPos = playerCamera.transform.forward;

        float sprintMul = GetSpeedMultiplier();

        Vector3 rawVelocity = Vector3.zero;
        if(!rb.isKinematic) rawVelocity = transform.TransformVector(inputHandler.GetMovementVector());
        camMov.tiltDirection = inputHandler.GetMovementVector();
        Vector3 nextVelocity = rawVelocity * (maxMovementSpeed * sprintMul * speedMultiplier * externalSpeedMultiplier);
        currentVelocity = Vector3.Lerp(currentVelocity, nextVelocity, acceleration * Time.deltaTime);
        
        speed = currentVelocity.magnitude;
        isMoving = speed > 0.2f;
    }

    public float GetSpeedMultiplier()
    {
        float sprintMul;
        if (!isSlowWalking)
            sprintMul = isSprinting ? sprintMultiplier : 1f;
        else
            sprintMul = slowWalkMultiplier;
        return sprintMul;
    }

    public void FixedUpdate()
    {
        if(rb.isKinematic)
            return;
        
        if (canMove)
            rb.MovePosition(transform.position + currentVelocity * Time.fixedDeltaTime);
        else
            currentVelocity = Vector3.zero;    

        if (!isOnGround)
            rb.AddForce(Vector3.down*additionalGravityForce, ForceMode.Acceleration);

        if (hasJumpedInThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            hasJumpedInThisFrame = false;
        }
    }

    public float movingSpeed => speed;
    public float externalSpeedMultiplier { get; set; }
}
