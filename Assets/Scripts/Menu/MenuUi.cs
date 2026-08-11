using UnityEngine;

public class MenuUi : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;

    private void Start()
    {
        MenuManager.Instance.MenuUi = this;
        loadingScreen.SetActive(false);
    }

    // ------------------- Public Methods -------------------

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}
