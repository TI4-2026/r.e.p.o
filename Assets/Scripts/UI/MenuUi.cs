using UnityEngine;
using UnityEngine.EventSystems;
public class MenuUi : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject creditsScreen1;
    [SerializeField] private GameObject creditsScreen2;
    [SerializeField] private GameObject creditsScreen3;
    [SerializeField] private GameObject startButton;

    private void Start()
    {
        MenuManager.Instance.MenuUi = this;
        loadingScreen.SetActive(false);
        optionsScreen.SetActive(false);
        creditsScreen1.SetActive(false);
        creditsScreen2.SetActive(false);
        creditsScreen3.SetActive(false);
        EventSystem.current.SetSelectedGameObject(startButton);
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

    public void ShowCreditsScreen1()
    {
        creditsScreen1.SetActive(true);
    }

    public void ShowCreditsScreen2()
    {
        creditsScreen2.SetActive(true);
    }

    public void ShowCreditsScreen3()
    {
        creditsScreen3.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void HideOptionsScreen()
    {
        optionsScreen.SetActive(false);
    }

    public void HideCreditsScreen1()
    {
        creditsScreen1.SetActive(false);
    }

    public void HideCreditsScreen2()
    {
        creditsScreen2.SetActive(false);
    }

    public void HideCreditsScreen3()
    {
        creditsScreen3.SetActive(false);
    }

    public void HideAllCredits()
    {
        creditsScreen1.SetActive(false);
        creditsScreen2.SetActive(false);
        creditsScreen3.SetActive(false);
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
