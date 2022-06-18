using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] PlayerWeaponController pwc;
    public LineRenderer laser;
    public float maxDistance;

    RaycastHit hit;
    float distance;
    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        laser.SetPosition(0, transform.position);
        if(Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            laser.SetPosition(1, hit.point);
            distance = Vector3.Distance(transform.position, hit.point);
        }
        else
        {
            laser.SetPosition(1, transform.position + transform.forward * maxDistance);
            distance = maxDistance;
        }
        pwc.aimPoint = laser.GetPosition(1);
        pwc.aimDistance = distance;
    }
}
