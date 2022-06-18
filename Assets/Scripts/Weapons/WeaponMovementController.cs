using UnityEngine;

public class WeaponMovementController : MonoBehaviour
{
    private WeaponItem _weaponStats;

    [Header("Sway Settings")]
    public float maxSway;
    public float swayFactor;
    public float swayRotateFactor;
    public float swayDamp;
    public float swayRotateDamp;

    [Header("Bob Settings")]
    public float walkRollFactor;
    public Vector3 bobAmount;
    public Vector3 bobSpeed;

    [Header("Breath Settings")]
    public float breathAmount;
    public float breathSpeed;

    [Header("Wall Push Settings")]
    public float wallCheckDistance;
    public float pushChangeSpeed;
    public float wallPushRotAngle;
    public LayerMask pushMask;

    [Header("Aimpunch Settings")]
    public float returnSpeed = 5f;
    public float changeSpeed = 10f;

    Vector3 position;
    public bool isLocked;

    //position
    private Vector3 swayAdditive;
    private Vector3 walkAdditive;
    private Vector3 breathAdditive;
    private Vector3 wallpushOffset;

    //rotation
    private Quaternion rotSwayAdditive;

    //recoil
    private Vector3 rotationRecoil;
    private Vector3 positionRecoil;
    private Vector3 rotDelta;

    // Use this for initialization
    void Start()
    {
        position = transform.localPosition;
    }
    public void InitStats(WeaponItem stats)
    {
        _weaponStats = stats;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, Time.deltaTime * returnSpeed);
        positionRecoil = Vector3.Lerp(positionRecoil, Vector3.zero, Time.deltaTime * returnSpeed);

        rotDelta = Vector3.Slerp(rotDelta, rotationRecoil, Time.deltaTime * changeSpeed);
        transform.localRotation = Quaternion.Euler(rotDelta) * rotSwayAdditive * Quaternion.Euler(new Vector3(wallpushOffset.z * wallPushRotAngle, 0, 0));
        transform.localPosition = position + walkAdditive + breathAdditive + swayAdditive + positionRecoil + wallpushOffset;
        if (!isLocked)
        {
            Sway();

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                Walk();
            else
                Breath();

            Wallpush();
        }
    }

    public void Sway()
    {
        float fX = Input.GetAxisRaw("Mouse X"),
              fY = Input.GetAxisRaw("Mouse Y"),
              rX = PlayerInputController.GetMovementVector().x;

        float clamped_fX = Mathf.Clamp(fX * swayFactor, -maxSway, maxSway),
              clamped_fY = Mathf.Clamp(fY * swayFactor, -maxSway, maxSway);

        Vector3 final = new Vector3(clamped_fX, clamped_fY, 0);

        Vector3 rotateWalk = Vector3.forward * (-rX * walkRollFactor); //slight roll to right/left when walking sideways

        Quaternion rotationX = Quaternion.AngleAxis(fY * swayRotateFactor, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(fX * swayRotateFactor, Vector3.up);

        swayAdditive = Vector3.Lerp(swayAdditive, final, swayDamp);
        rotSwayAdditive = Quaternion.Lerp(rotSwayAdditive, rotationX * rotationY * Quaternion.Euler(rotateWalk), swayRotateDamp);
    }

    public void Walk()
    {
        float xBob = 2 * Mathf.Sin(Time.time * bobSpeed.x + Mathf.PI / 2) * bobAmount.x;
        float yBob = Mathf.Sin(Time.time * 2 * bobSpeed.y) * bobAmount.y;
        float zBob = Mathf.Cos(Time.time * 2 * bobSpeed.z) * bobAmount.z;

        Vector3 final = new Vector3(xBob, yBob, zBob);
        walkAdditive = Vector3.Lerp(walkAdditive, final, 0.1f * Time.deltaTime);
    }
    public void Wallpush()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, wallCheckDistance, pushMask.value))
        {
            wallpushOffset = Vector3.Lerp(wallpushOffset, Vector3.back * (wallCheckDistance - hit.distance), Time.deltaTime * pushChangeSpeed);
        }
        else
        {
            wallpushOffset = Vector3.Lerp(wallpushOffset, Vector3.zero, Time.deltaTime * pushChangeSpeed);
        }
    }

    public void Breath()
    {
        walkAdditive = Vector3.Lerp(walkAdditive, Vector3.zero, Time.deltaTime * breathSpeed);
        float yBob = Mathf.Sin(Time.time * 2 * breathSpeed) * breathAmount;

        Vector3 final = new Vector3(0, yBob, 0);
        breathAdditive = Vector3.Lerp(breathAdditive, final, 0.1f * Time.deltaTime);
    }

    public void Punch(float coef)
    {
        float fX = Random.Range(-_weaponStats.horizontalRotPunch, _weaponStats.horizontalRotPunch);
        float fY = Random.Range(-_weaponStats.verticalRotPunch * _weaponStats.negativeVerticalReduction, _weaponStats.verticalRotPunch);

        Vector3 rotate = Vector3.up * fX + Vector3.left * fY;
        Vector3 pushBack = Vector3.back * _weaponStats.pushBackForce*coef;

        rotationRecoil += rotate * coef;
        positionRecoil = pushBack;
    }
}
