using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction: MonoBehaviour
{
    [Header("Interactions")]
    [SerializeField] private string moveObjectLetter = "E";
    [SerializeField] private string moveObjectDesc = "Move Object";

    [Header("References")]
    [SerializeField] private CinemachineInputAxisController cameraInputAxisController;
    [SerializeField] private GameObject cameraHandle;

    private PlayerMovement playerMovement;
    private Hud hud;
    private bool isCursorLocked = true;

    private GameObject moveObject;  
    private Action moveObjectEnable;
    private Action moveObjectDisable;
    private bool isMoveObjectMode = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        hud = GameManager.Instance.Hud;
        LockCursor();

        TimeTravel.OnTimeChange += RevokeInteraction;
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

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (TryMoveObject()) return;
    }

    // ----------------- Public Methods -----------------

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PossibilityToMoveObject(GameObject go, Action enable, Action disable)
    {
        moveObject = go;
        moveObjectEnable = enable;
        moveObjectDisable = disable;

        hud.EnableInteraction(moveObjectLetter, moveObjectDesc);
    }

    public void RemovePossibilityToMoveObject(GameObject go)
    {
        if (isMoveObjectMode) return;
        if (moveObject != null && moveObject.Equals(go)) 
        {
            moveObject = null;
            moveObjectEnable = null;
            moveObjectDisable = null;
            hud.DisableInteraction();
        }
    }

    public void RevokeInteraction(bool isFuture) { RevokeInteraction(); }
    public void RevokeInteraction()
    {
        if (!isMoveObjectMode) return;
        hud.DisableInteraction();
        TryMoveObject();
    }

    public bool TryMoveObject()
    {
        if (isMoveObjectMode)
        {
            if (moveObject != null)
            {
                isMoveObjectMode = false;
                playerMovement.SetMoveObjectMode(false, null);
                playerMovement.SetDefaultSpeed();
                moveObjectDisable?.Invoke();
                moveObject = null;
                return true;
            }
        }else
        {
            if (moveObject != null)
            {
                isMoveObjectMode = true;
                playerMovement.SetMoveObjectMode(true, moveObject);
                playerMovement.SetSpeed(playerMovement.GetSpeed()/2);
                moveObjectEnable?.Invoke();
                return true;
            }
        }  
        return false;

        // Retorna true se alguma interacao foi comecada ou terminada. False caso contrario.
        // Motivo: Na funcao interact(), ela vai usar esse return para saber se houve alguma acao.
    }
    
    // ----------------- Private Methods -----------------

    private void SetCameraInputEnabled(bool enabled)
    {
        cameraInputAxisController.enabled = enabled;
    }
}