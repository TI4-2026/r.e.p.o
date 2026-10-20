using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
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
            .setDelay(i * 0.3f).setOnComplete(() =>
            {
                UIGlitch glitch = pauseMenu.GetComponent<UIGlitch>();
                glitch.PlayGlitch();
            });
    }
}

    public void ResumeGame()
    {
        UIGlitch glitch = pauseMenu.GetComponent<UIGlitch>();
        glitch.ChangeGlitch();

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenOptionsMenu()
    {
        UIGlitch glitch = optionsMenu.GetComponent<UIGlitch>();
        glitch.PlayGlitch();
        optionsMenuOpen = true;
    }

    public void CloseOptionsMenu()
    {
        UIGlitch glitch = optionsMenu.GetComponent<UIGlitch>();
        glitch.ChangeGlitch();
        optionsMenuOpen = false;
    }

    public void GoToMainMenu()
    {
        UIGlitch glitch = pauseMenu.GetComponent<UIGlitch>();
        glitch.ChangeGlitch();
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
}