using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    // -------------- Singleton --------------
    public static InfoManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    // -------------- Variables --------------

    [SerializeField] private GameObject cardList;
    [SerializeField] private GameObject cardPrefab;

    // -------------- Default Methods --------------
    

    // -------------- Public Methods --------------

    public void CreateCard(CardData data)
    {
        GameObject card = Instantiate(cardPrefab, cardList.transform);
        CardBehavior cardBehavior = card.GetComponent<CardBehavior>();
        
        cardBehavior.InitializeCard(data);
    }

    // -------------- Private Methods --------------
}
