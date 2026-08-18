using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerCollisionSelf : MonoBehaviour
{   
    /*
        This script is used to manage collisions between the player and objects.
        The Character Controller naturally triggers OnControllerColliderHit, but does not trigger OnCollisionEnter on the other Game Object.
        Therefore, we use this script to handle collisions.
    */
    

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        try
        {
            hit.gameObject.GetComponent<PlayerCollisionObject>().OnPlayerCollisionEnter(hit.gameObject);
        }
        catch
        {
            
        }

    }
}
