using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK74SU_animEvents : MonoBehaviour
{
    public WeaponController wc;

    public void EndReload()
    {
        wc.RefreshAmmo();
        wc.animator.SetBool("reload", false);
    }

    public void CheckEvent(int wpn)
    {
        //wc.player.ui.ShowAmmoCounter(wpn != 0);
        wc.animator.SetBool("check",false);
    }
}
