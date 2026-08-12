using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private string gameSceneName;

    [Header("Auto-Filled")]
    public MenuUi MenuUi;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // ------------------- Public Methods -------------------

    public void StartGame()
    {
        Debug.Log("Loading game scene");
        LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    // ------------------- Private Methods -------------------

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        MenuUi.ShowLoadingScreen();
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
