using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float mouseSensitivity = 0.5f;
    public bool mouseLock = false;
    public bool movementLock = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCursorState(CursorLockMode lockMode, bool visibility)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visibility; 
    }

    public float GetMouseHorizontal()
    {
        if(!mouseLock)
            return Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        return 0;
    }

    public float GetMouseVertical()
    {
        if (!mouseLock)
            return Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        return 0;
    }

    public Vector3 GetMovementVector()
    {
        if (!movementLock)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 move = new Vector3(x, 0, z);

            return move.normalized;
        }
        return Vector3.zero;
    }
}
