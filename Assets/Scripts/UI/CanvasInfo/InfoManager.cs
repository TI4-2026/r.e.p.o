using System.Collections.Generic;
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
    
    /*
     * Script para controlar o Canvas_Info (parte da hud para mostrar informações na tela).
     * Na teoria já vai estar configurado no prefab do canvas. Senão, apenas coloque o script e preencha as variaveis.
     * As funções contêm os seus nomes autoexplicativos.
     */
    
    // -------------- Variables --------------

    [SerializeField] private float maxActivesCards;
    [SerializeField] private GameObject cardList;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> cardsActive;

    // -------------- Default Methods --------------

    private void Start()
    {
        cardsActive = new List<GameObject>();
    }

    // -------------- Public Methods --------------
    
    public void CreateCard(CardData data)
    {
        if (cardsActive.Count >= maxActivesCards)
        {
            cardsActive[0].GetComponent<CardBehavior>().ForceRemoveCard();
        }
        
        GameObject card = Instantiate(cardPrefab, cardList.transform);
        card.GetComponent<CardBehavior>().InitializeCard(data);
        
        cardsActive.Add(card);
    }
    
    // Chamado exclusivamente pelo CardBehavior.cs.
    public void RemoveCard(GameObject card)
    {
        cardsActive.Remove(card);
    }

    // -------------- Private Methods --------------
}
