using System;
using UnityEngine;

public class InfoTrigger : MonoBehaviour
{
    [SerializeField] private string tipText;
    [SerializeField] private float tipDuration;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InfoManager.Instance.CreateTip(tipText, tipDuration > 0 ? tipDuration : 5f);
            Destroy(gameObject);
        }
    }
}