using System;
using UnityEngine;

public class EnableObjectOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject objectToEnable;

    private void Start()
    {
        objectToEnable.SetActive(false);
    }

    private void OnDestroy()
    {
        objectToEnable.SetActive(true);
    }
}
