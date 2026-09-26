using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject[] PauseComponents;

    bool optionsMenuOpen = false;

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseMenu.activeSelf == false)
            {
                pauseMenu.SetActive(true);
                AnimarComponentes();

                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else if (pauseMenu.activeSelf == true && optionsMenuOpen == false)
            {
                pauseMenu.SetActive(false);

                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void AnimarComponentes()
{
    for (int i = 0; i < PauseComponents.Length; i++)
    {
        GameObject componente = PauseComponents[i];

        componente.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        LeanTween.scale(componente, Vector3.one, 0.5f)
            .setEase(LeanTweenType.easeOutBack)
            .setIgnoreTimeScale(true)
            .setDelay(i * 0.3f);
    }
}

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenOptionsMenu()
    {
        optionsMenuOpen = true;
    }

    public void CloseOptionsMenu()
    {
        optionsMenuOpen = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
}