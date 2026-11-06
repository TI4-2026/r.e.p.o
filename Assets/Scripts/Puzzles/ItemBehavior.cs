using System;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool enableExitOnCollect = true;
    
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectGear(enableExit: enableExitOnCollect);
            Destroy(gameObject);
        }
    }
}
