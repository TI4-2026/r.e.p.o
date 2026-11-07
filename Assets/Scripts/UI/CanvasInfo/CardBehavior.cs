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

    private float animationDuration = 0.5f;
    
    // -------------- Default Methods --------------

    private void Start()
    {
        rectSelf = GetComponent<RectTransform>();
        rectWrapper = cardWrapper.GetComponent<RectTransform>();

        rectWrapper.position = new Vector3(
            -rectWrapper.sizeDelta.x/2,
            rectWrapper.position.y
            );
        ShowCard();
        StartCoroutine(I_WaitDuration());
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
    
    // -------------- Private Methods --------------

    private IEnumerator I_WaitDuration()
    {
        yield return new WaitForSeconds(data.duration);
        HideCard();
    }

    private void ShowCard()
    {
        rectWrapper.LeanMoveX(rectWrapper.sizeDelta.x/2, animationDuration)
            .setEaseOutQuad();
    }

    private void HideCard()
    {
        rectWrapper.LeanMoveX(-rectWrapper.sizeDelta.x/2, animationDuration)
            .setEaseOutQuad();
        Destroy(gameObject, animationDuration);
    }
    
}
