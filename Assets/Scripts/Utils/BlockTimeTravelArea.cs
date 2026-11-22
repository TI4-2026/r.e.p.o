using System;
using UnityEngine;

public class BlockTimeTravelArea : MonoBehaviour
{
    [Tooltip("Dialogo que sera mostrado quando o player tentar mudar de tempo na regiao bloqueada.")]
    [SerializeField] DialogData dialog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimeTravel.Instance.BlockTimeTravel(gameObject, dialog);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimeTravel.Instance.UnblockTimeTravel(gameObject);
        }
    }
}
