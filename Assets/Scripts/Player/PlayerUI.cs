using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUI : MonoBehaviour
{
    public bool uiMode;

    [SerializeField] InventoryManagerUI inventoryUI;
    [HideInInspector] public UnityEvent<bool> OnUIModeChanged;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            uiMode = inventoryUI.ActivateUI();
            OnUIModeChanged.Invoke(uiMode);
        }
    }
}
