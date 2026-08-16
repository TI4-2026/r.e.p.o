using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCooldown = 1f;


    // --------------- Input Actions ---------------

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Dash");
        }
    }

    // --------------- Public Methods ---------------




    // --------------- Private Methods ---------------



}
