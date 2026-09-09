using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionSelf : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private List<ControllerColliderHit> previousColliders;
    private List<ControllerColliderHit> colliders;

    public UnityEvent ccMoved;

    void Start()
    {
        playerMovement  = gameObject.GetComponent<PlayerMovement>();
        playerMovement.SetPlayerCollisionSelf(this);

        colliders = new List<ControllerColliderHit>();
        previousColliders = new List<ControllerColliderHit>();

        ccMoved.AddListener(RenewColliders);
    }

    void RenewColliders()
    {      
        bool[] resultColliders = new bool[colliders.Count]; // false: enter | true: stay
        bool[] resultPColliders = new bool[previousColliders.Count]; // false: exit | true: stay
 
        for (int i=0; i<colliders.Count; i++)
        {
            for (int j=0; j<previousColliders.Count; j++)
            {
                if (colliders[i].Equals(previousColliders[j]))
                {
                    resultColliders[i] = true;
                    resultPColliders[j] = true;
                }
            }
        }   
        
        for (int i=0; i<colliders.Count; i++)
        {
            if (!resultColliders[i])
            {
                if (colliders[i].gameObject.TryGetComponent(out PlayerCollisionObject pc))
                {
                    pc.OnPlayerCollisionEnter(gameObject);
                }
            }
        }
        for (int i=0; i<previousColliders.Count; i++)
        {
            if (!resultPColliders[i])
            {
                if (previousColliders[i].gameObject.TryGetComponent(out PlayerCollisionObject pc))
                {
                    pc.OnPlayerCollisionExit(gameObject);
                }
            }
        }

        previousColliders = new List<ControllerColliderHit>(colliders);
        colliders.Clear();
    }

    // Called after "CharacterController.Move()" in PlayerMovement.cs
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        colliders.Add(hit);
    }
}
