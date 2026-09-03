using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera: MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController cameraInputAxisController;
    [SerializeField] private GameObject cameraHandle;

    private PlayerMovement playerMovement;
    private bool isCursorLocked = true;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        LockCursor();
    }

    private void Update() {
        cameraHandle.transform.position = transform.position;
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