using System;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            playerInteraction.CollectGear();
            Destroy(gameObject);
        }
    }
}
