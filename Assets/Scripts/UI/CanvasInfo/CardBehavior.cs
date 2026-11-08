using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour
{
    [SerializeField] private GameObject cardWrapper;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI cardDescription;

    private RectTransform rectSelf;
    private RectTransform rectWrapper;
    private CardData data;
    private Coroutine activeCoroutine;

    private float animationDuration = 0.5f;
    
    /*
     * Script do card do Canvas_Info. Não deve ser chamado nada aqui e sim através do InfoManager.cs.
     * Ou seja, não precisa mexer nele diretamente.
     * Funções na maioria tem seus nomes autoexplicativos.
     */
    
    // -------------- Default Methods --------------

    private void Start()
    {
        rectSelf = GetComponent<RectTransform>();
        rectWrapper = cardWrapper.GetComponent<RectTransform>();
        
        // Coloca o card fora da tela (O pai é controlado pelo Vertical Layout. Então controla a posição do filho para isso).
        rectWrapper.position = new Vector3(
            -rectWrapper.sizeDelta.x/2,
            rectWrapper.position.y
            );
        
        
        ShowCard();
        activeCoroutine = StartCoroutine(I_WaitDuration());
    }

    private void Update()
    {
        rectSelf.sizeDelta = new Vector2(10, rectWrapper.sizeDelta.y);
    }

    // -------------- Public Methods --------------
    
    public void InitializeCard(CardData data)
    {
        this.data = data;
        cardTitle.text = data.title;
        cardDescription.text = data.description;
    }

    public void ForceRemoveCard()
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        HideCard();
    }
    
    // -------------- Private Methods --------------

    private IEnumerator I_WaitDuration()
    {
        yield return new WaitForSeconds(data.duration);
        HideCard();
    }
    
    // Animação de aparecer o card (slide para direita)
    private void ShowCard()
    {
        rectWrapper.LeanMoveX(rectWrapper.sizeDelta.x/2, animationDuration)
            .setEaseOutQuad();
    }
    
    // Animação de desaparecer o card (slide para esquerda).
    private void HideCard()
    {
        rectWrapper.LeanMoveX(-rectWrapper.sizeDelta.x/2, animationDuration)
            .setEaseOutQuad()
            .setOnComplete(DestroyCard);
    }
    
    // Função para remover o card.
    private void DestroyCard()
    {
        InfoManager.Instance.RemoveCard(gameObject);
        Destroy(gameObject, animationDuration);
    }
    
}
