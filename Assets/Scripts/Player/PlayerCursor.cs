using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController cameraInputAxisController;

    private PlayerMovement playerMovement;
    private bool isCursorLocked = true;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        LockCursor();
    }

    // ----------------- Input Actions -----------------

    public void AlternateCursor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCursorLocked = !isCursorLocked;

            if (isCursorLocked)
            {
                UnlockCursor();
                playerMovement.SetMovementEnabled(false);
                SetCameraInputEnabled(false);
            }
            else
            {
                LockCursor();
                playerMovement.SetMovementEnabled(true);
                SetCameraInputEnabled(true);
            }
        }
    }

    // ----------------- Private Methods -----------------

    private void SetCameraInputEnabled(bool enabled)
    {
        cameraInputAxisController.enabled = enabled;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}