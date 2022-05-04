using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Equipment Vars")]
    public GameObject[] equpimentList;
    
    [Header("Grenade Vars")]
    public float throwPower = 200f;

    [Header("Switching Vars")]
    private int currentSwitchTo = -1;
    public bool safeToNextSwitch = true;
    public bool equipmentDeactivated = true;
    
    [Header("References")]
    public InventoryController inv;
    public Player player;
    public GameObject hand;
    private Animator handAnimator;
    private Transform cam;

    private void Start()
    {
        player = GetComponent<Player>();
        cam = player.cam.transform;
        handAnimator = hand.GetComponent<Animator>();
        hand.SetActive(false);
    }

    public void ExpireEquipment()
    {
        inv.DiscardSelectedItem();
        HideEquipment(inv.currentHotbarSlot);
    }


    public void SwitchEquipment(int to)
    {
        if(to == -1) return;
        
        currentSwitchTo = to;
        if (equipmentDeactivated)
        {
            equipmentDeactivated = false;
            hand.SetActive(true);
            ActivateEquipment(currentSwitchTo);
            handAnimator.SetBool("state", true);
        }
        else if (safeToNextSwitch)
        {
            StartCoroutine(SwitchEquipmentAnimation());
        }
    }

    IEnumerator SwitchEquipmentAnimation()
    {
            safeToNextSwitch = false;
            handAnimator.SetBool("state", false);
            yield return new WaitForSeconds(0.4f);
            ActivateEquipment(currentSwitchTo);
            handAnimator.SetBool("state", true);
            yield return new WaitForSeconds(0.2f);
            safeToNextSwitch = true;
            yield return null;
        
    }
    
    IEnumerator ThrowAnimation()
    {
        safeToNextSwitch = false;
        handAnimator.SetBool("throw", true);
        yield return new WaitForSeconds(0.5f);
        handAnimator.SetBool("throw", false);
        yield return new WaitForSeconds(0.3f);
        safeToNextSwitch = true;
        yield return null;
        
    }
    
    IEnumerator HideEquipmentAnimation(int id)
    {
        safeToNextSwitch = false;
        handAnimator.SetBool("state", false);
        yield return new WaitForSeconds(0.5f);
        equpimentList[id].SetActive(false);
        hand.SetActive(false);
        safeToNextSwitch = true;
        yield return null;
        
    }
    
    public IEnumerator FakeHideEquipmentAnimation(bool overrideSafe)
    {
        safeToNextSwitch = false;
        handAnimator.SetBool("state", false);
        yield return new WaitForSeconds(0.5f);
        if(overrideSafe) safeToNextSwitch = true;
        yield return null;
        
    }

    public void ActivateEquipment(int id)
    {
        for (int i = 0; i < equpimentList.Length; i++)
        {
            if (i == id)
            {
                if(equpimentList[i] != null) equpimentList[i].SetActive(true);
            }
            else
            {
                if(equpimentList[i] != null) equpimentList[i].SetActive(false);
            }
        }
    }

    public void HideEquipment(int id)
    {
        if (safeToNextSwitch)
        {
            handAnimator.SetBool("state", false);
            StartCoroutine(HideEquipmentAnimation(id));
            equipmentDeactivated = true;
        }
    }
    
    public void HideEquipment()
    {
        if (safeToNextSwitch)
        {
            handAnimator.SetBool("state", false);
            for (int i = 0; i < equpimentList.Length; i++)
            {
                if(equpimentList[i] != null) equpimentList[i].SetActive(false);
            }
            equipmentDeactivated = true;
        }
    }

    public IEnumerator PlayDrinkingAnimation(bool overrideSafe)
    {
        safeToNextSwitch = false;
        handAnimator.SetBool("drink", true);
        yield return new WaitForSeconds(0.85f);
        if(overrideSafe) safeToNextSwitch = true;
        handAnimator.SetBool("drink", false);
        yield return null;
    }
    
    
}
