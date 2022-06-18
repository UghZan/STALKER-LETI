using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    [SerializeField] Camera playerCamera;
    [SerializeField] Player owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = playerCamera.ViewportPointToRay(Vector2.one * 0.5f);
            if(Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                if(hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteract();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            owner.weaponModule.EquipWeapon();
        }
    }
}
