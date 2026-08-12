using UnityEngine;
using UnityEngine.EventSystems;
public class MenuUi : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject optionsScreen;

    private void Start()
    {
        MenuManager.Instance.MenuUi = this;
        loadingScreen.SetActive(false);
        optionsScreen.SetActive(false);
    }

    // ------------------- Public Methods -------------------

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void ShowOptionsScreen()
    {
        optionsScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void HideOptionsScreen()
    {
        optionsScreen.SetActive(false);
    }

    // ------------------- Animations -------------------

    public void HoverButton(BaseEventData eventData)
    {
        if (eventData is not PointerEventData pointerData || pointerData.pointerEnter == null)
            return;

        GameObject button = pointerData.pointerEnter;
        LeanTween.scale(button, new Vector3(1.1f, 1.1f, 1.1f), 0.2f).setEaseInOutSine();
    }

    public void UnhoverButton(BaseEventData eventData)
    {
        if (eventData is not PointerEventData pointerData || pointerData.pointerEnter == null)
            return;

        GameObject button = pointerData.pointerEnter;
        LeanTween.scale(button, Vector3.one, 0.2f).setEaseInOutSine();
    }

    public void ClickButton(BaseEventData eventData)
    {
        if (eventData is not PointerEventData pointerData || pointerData.pointerPress == null)
            return;

        GameObject button = pointerData.pointerPress;
        LeanTween.scale(button, new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.scale(button, Vector3.one, 0.1f).setEaseInOutSine();
        });
    }
}
