using System.Collections;
using System.Collections.Generic;
using Common;
using Inventory;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Stamina")] 
    public float maxStamina = 100f;
    public float stamina;
    public float staminaDrainCoefficient = 1.0f;
    public float staminaRegenCoefficient = 1.0f;
    private float staminaRegenCooldownTimer;
    
    [Header("Weapons")] 
    public int ak47ammo = 30;
    public int asvalammo = 0;

    [Header("Passive Upgrades")] 
    public bool hasBackpack = false;
    public bool hasPodsumok1 = false;
    public bool hasPodsumok2 = false;
    public bool hasRazgruzka = false;
    public bool hasSteroids = false;
    public bool hasDetector = false;
    public bool hasVAL = false;

    [Header("Pickup Settings")] 
    public float pickupRange = 2f;
    public float pickupRadius = 0.5f; //spherecast

    [Header("Visual Vars")] 
    private float invProgress = 0;
    public Image invProgressGauge;
    public Image staminaGauge;
    public bool invOpen = false;

    [Header("Misc")] public static float anomalyLoadDistance = 50f;

    [Header("References")] 
    public GameObject flashlight;
    public PlayerStats s;
    public Camera cam;
    public PlayerController pc;
    public UIController ui;
    public Collider pickupZone;
    public InventoryController inv;
    public BuffManager buffM;
    public EquipmentManager equip;
    private static readonly int MulColor = Shader.PropertyToID("mulColor");

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        cam = Camera.main;
        s = GetComponent<PlayerStats>();
        buffM = GetComponent<BuffManager>();
        equip = GetComponent<EquipmentManager>();
        stamina = maxStamina;
        
    }
    // Update is called once per frame
    void Update()
    {
        //Stamina calculations
        if (pc.isSprinting)
        {
            stamina -= Time.deltaTime * staminaDrainCoefficient;
            staminaRegenCooldownTimer = 1.0f;
        }
        else
        {
            if (staminaRegenCooldownTimer > 0)
                staminaRegenCooldownTimer -= Time.deltaTime;
            else
            {
                if(stamina < maxStamina)
                    stamina += Time.deltaTime * staminaRegenCoefficient * (pc.isMoving ? 0.33f : 1f);
            }
        }
        
        //Picking up
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.SphereCast(ray, pickupRadius, out RaycastHit hit, pickupRange))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteract(this);
                }
            }
        }
        
        //Flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeInHierarchy);
        }
        
        //Take out bolt
        if (Input.GetKeyDown(KeyCode.B))
        {
            inv.ChangeToSpecialSlot(60);
        }
        
        //Take out weapon
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (hasVAL) inv.ChangeToSpecialSlot(62);
            else inv.ChangeToSpecialSlot(61);
        }

        //Switching hotbar slots
        if (inv.equip.safeToNextSwitch)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                inv.ChangeToHotbarSlot(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                inv.ChangeToHotbarSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                inv.ChangeToHotbarSlot(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                inv.ChangeToHotbarSlot(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                inv.ChangeToHotbarSlot(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                inv.ChangeToHotbarSlot(5);
            }
        }

        //Dropping items
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(!inv.ui.dragging)
            {
                ItemInfo temp = inv.GetCurrentSelectedItem();
                if (temp != null)
                {
                    inv.DiscardSelectedItem();
                    MiscHelper.instance.GeneratePickupAtPosition(temp, pc.playerLegs.transform.position);
                }
            }
            else
            {
                ItemInfo temp = null;
                if (inv.ui.startDragSlot < 4)
                {
                    temp = inv.GetItemInHotbarSlot(inv.ui.startDragSlot);
                    inv.DiscardItemInHotbarSlot(inv.ui.startDragSlot);
                }
                else
                {
                    temp = inv.GetItemInInvSlot(inv.ui.startDragSlot % 4);
                    inv.DiscardItemInInvSlot(inv.ui.startDragSlot % 4);
                }

                MiscHelper.instance.GeneratePickupAtPosition(temp, pc.playerLegs.transform.position);
                inv.ui.dragging = false;
                inv.ui.dragDummy.SetActive(false);
            }
        }
        
        //Inventory opening
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inv.invMode)
                invProgressGauge.gameObject.SetActive(true);
            else
            {
                invOpen = false;
                SwitchInvMode(false);
            }
        }
        
        if (Input.GetKey(KeyCode.I))
        {
            if(!inv.invMode && equip.equipmentDeactivated)
                if (invProgress < 2)
                {
                    invProgressGauge.fillAmount = invProgress / 2;
                    invProgress += Time.deltaTime * (pc.isMoving ? 0.5f : 1f) * (pc.isSprinting ? 0.75f : 1f);
                }
                else
                {
                    invOpen = true;
                    SwitchInvMode(true);
                    invProgressGauge.gameObject.SetActive(false);
                }
        }
        
        //Reset inv
        if (Input.GetKeyUp(KeyCode.I))
        {
            invProgressGauge.gameObject.SetActive(false);
            invProgress = 0;
        }
    }
    public void UpdateSpecials(int item)
    {
        switch (item)
        {
            case -3:
                ak47ammo += Random.Range(25,31);
                break;
            case -2:
                asvalammo += Random.Range(15,21);
                break;
            case 0:
                hasBackpack = true;
                inv.AddMaxInvSlots(2);
                break;
            case 1:
                hasRazgruzka = true;
                inv.AddMaxHotSlots(3);
                break;
            case 2:
                hasPodsumok1 = true;
                inv.AddMaxInvSlots(2);
                break;
            case 3:
                hasPodsumok2 = true;
                inv.AddMaxInvSlots(2);
                break;
            case 4:
                hasSteroids = true;
                pc.speedMultiplier = 1.2f;
                break;
            case 5:
                hasDetector = true;
                break;
            case 6:
                hasVAL = true;
                break;
        }
    }
    public void AddBuff(BuffEntry b)
    {
        buffM.AddBuff(b);
    }
    public void AddBuff(Buff b, int time)
    {
        buffM.AddBuff(new BuffEntry(s, b, time, buffM));
    }

    public bool CheckBuff(Buff b)
    {
        return buffM.buffs.Exists(entry => entry.buff == b);
    }
    void SwitchInvMode(bool state)
    {
        inv.ControlInvMode(state);
        pc.ControlMouseLock(state);
    }
}
