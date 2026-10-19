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
        UIGlitch glitch = loadingScreen.GetComponent<UIGlitch>();
        glitch.PlayGlitch();
    }

    public void ShowOptionsScreen()
    {
        optionsScreen.SetActive(true);
        UIGlitch glitch = optionsScreen.GetComponent<UIGlitch>();
        glitch.PlayGlitch();
    }

    public void ShowCreditsScreen1()
    {
        creditsScreen1.SetActive(true);
        UIGlitch glitch = creditsScreen1.GetComponent<UIGlitch>();
        glitch.PlayGlitch();
    }

    public void ShowCreditsScreen2()
    {
        creditsScreen2.SetActive(true);
        UIGlitch close = creditsScreen1.GetComponent<UIGlitch>();
        close.ChangeGlitch();
        UIGlitch glitch = creditsScreen2.GetComponent<UIGlitch>();
        glitch.PlayGlitch();
    }

    public void ShowCreditsScreen3()
    {
        creditsScreen3.SetActive(true);
        UIGlitch close = creditsScreen2.GetComponent<UIGlitch>();
        close.ChangeGlitch();
        UIGlitch glitch = creditsScreen3.GetComponent<UIGlitch>();
        glitch.PlayGlitch();
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void HideOptionsScreen()
    {
        UIGlitch close = optionsScreen.GetComponent<UIGlitch>();
        close.ChangeGlitch();
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
        UIGlitch[] close = new UIGlitch[3];
        close[0] = creditsScreen1.GetComponent<UIGlitch>();
        close[1] = creditsScreen2.GetComponent<UIGlitch>();
        close[2] = creditsScreen3.GetComponent<UIGlitch>();
        for(int i = 0; i < close.Length; i++)
        {
            if (close[i].gameObject.activeInHierarchy)
            {
                close[i].ChangeGlitch();
            }
        }
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
