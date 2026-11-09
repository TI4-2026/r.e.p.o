using System;
using UnityEngine;
using UnityEngine.UI;

public class UiCardTrigger : MonoBehaviour
{
    [SerializeField] private CardData CardData;


    // -------------- Default Methods --------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InfoManager.Instance.CreateCard(CardData);
            Destroy(gameObject);
        }
    }

    // -------------- Public Methods --------------
    

    // -------------- Private Methods --------------
}