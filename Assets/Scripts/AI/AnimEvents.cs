using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public GameObject damageZone;

    public void SetDamageZoneState(int state)
    {
        damageZone.SetActive(state != 0);
    }
}
