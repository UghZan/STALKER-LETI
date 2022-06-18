using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMotor : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed;
    public float maxAcceleration;
    public float sprintMultiplier;

    [Header("Gravity Settings")]
    public float gravity;
    public float groundCheckDistance;

    [Header("Dash Settings")]
    public float dashCooldown;
    public float dashDistance;
    public float dashSpeed;
    public int maxDashes;
    private float dashTimer;
    private int dashes;

    [Header("Climb Settings")]
    public float ladderClimbSpeed;

    [Header("Mouse Settings")]
    public float sensitivity = 10f;
    public float minHeadAngle = -90;
    public float maxHeadAngle = 60;

    [Header("Stats")]
    public bool isSprinting;
    public bool isDashing;
    private bool isLocked;
    public bool isGrounded;
    public bool isOnLadder;

    public Vector3 velocity;
    public float Speed => velocity.magnitude;

    private Vector3 inputVelocity;
    private Vector2 inputRotation;
    public Vector3 lastDashVelocity { get; private set; }
    private float rotateHead;
    private float rotateBody;

    [HideInInspector] public UnityEvent OnDash;

    [Header("References")]
    [SerializeField] private CharacterController charControl;
    [SerializeField] private Transform head;
    [SerializeField] private Player owner;

    public void Start()
    {
        dashes = maxDashes;
        owner.uiModule.OnUIModeChanged.AddListener((lockMode) => isLocked = lockMode);
    }

    void Update()
    {
        if (!isDashing && !isLocked)
        {
            CheckGround();
            RotateHead();
            RotateBody();
            Move();
            DashControl();
        }

        if (dashTimer >= 0) dashTimer -= Time.deltaTime;
        else
        {
            dashes = Mathf.Min(dashes + 1, maxDashes);
            if (dashes < maxDashes) dashTimer = dashCooldown;
        }
    }

    private void CheckGround()
    {
        if (isOnLadder) { isGrounded = true; return; }

        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    void DashControl()
    {
        if (isOnLadder) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashes > 0) Dash();

        if (Input.GetKey(KeyCode.LeftShift) && !isDashing)
        {
            //Debug.Log("sprinting");
            isSprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;
    }

    void Dash()
    {
        StartCoroutine(DashTranslation());
    }

    void Move()
    {
        float multiplier = isSprinting ? sprintMultiplier : 1f;
        Vector3 desiredVelocity = PlayerInputController.GetMovementVector() * (maxSpeed * multiplier);
        float maxChangeSpeed = maxAcceleration * Time.deltaTime;

        velocity = Vector3.Lerp(velocity, desiredVelocity, maxChangeSpeed);
        if (!isOnLadder)
        {
            if (isGrounded)
            {
                velocity.y = 0;
            }
            else
            {
                velocity.y = -gravity;
            }
        }

        if(isOnLadder)
        {
            if (Input.GetKey(KeyCode.Space))
                velocity.y = ladderClimbSpeed;
            if (Input.GetKey(KeyCode.LeftControl))
                velocity.y = -ladderClimbSpeed;
        }

        charControl.Move(transform.TransformVector(velocity) * Time.deltaTime);
    }

    void RotateHead()
    {
        float diff = Input.GetAxisRaw("Mouse Y");

        rotateHead -= diff;
        rotateHead = Mathf.Clamp(rotateHead, minHeadAngle, maxHeadAngle);
        head.localEulerAngles = new Vector3(rotateHead, 0, 0);
    }

    void RotateBody()
    {
        float diff = Input.GetAxisRaw("Mouse X");

        rotateBody += diff;
        transform.localEulerAngles = new Vector3(0, rotateBody, 0);
    }

    IEnumerator DashTranslation()
    {
        isDashing = true;
        Vector3 dashTarget = transform.position;

        if (Physics.SphereCast(transform.position, 0.5f, transform.TransformDirection(velocity.normalized), out RaycastHit hit, dashDistance))
            dashTarget = hit.point + hit.normal * 0.5f;
        else
            dashTarget = transform.position + transform.TransformDirection(velocity.normalized) * dashDistance;

        lastDashVelocity = transform.TransformDirection((dashTarget - transform.position).normalized);
        OnDash.Invoke();

        while (Vector3.Distance(dashTarget, transform.position) >= dashSpeed * Time.deltaTime)
        {
            //Debug.Log("position: " + transform.position + ", target: " + dashTarget);
            charControl.Move((dashTarget - transform.position).normalized * (dashSpeed * Time.deltaTime));
            yield return null;
        }

        transform.position = dashTarget;
        isDashing = false;
        dashes--;
        dashTimer = dashCooldown;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
            isOnLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
            isOnLadder = false;
    }
}
