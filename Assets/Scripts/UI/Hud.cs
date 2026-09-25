using System;
using System.Collections;
using TMPro;
using Unity.InferenceEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private Color fadeColor = Color.black;

    [Header("References")]
    [SerializeField] private Image panelFade;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image deathPanel;
    [SerializeField] private TextMeshProUGUI chancesText;

    private Coroutine fadeCoroutine;

    // ----------- Unity Methods -----------
    
    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Hud = this;
        }
    }

    private void Start()
    {
        panelFade.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
    }

    // ----------- Public Methods -----------

    public void BlackFade(Action onMiddle = null, Action onComplete = null)
    {
        if (panelFade == null)
        {
            onMiddle?.Invoke();
            onComplete?.Invoke();
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        
        fadeCoroutine = StartCoroutine(I_BlackFade(onMiddle, onComplete));
    }

    public void FlashHealthBar(Color color, float life, float maxLife)
    {
        LeanTween.value(healthBar.gameObject, color, Color.white, 0.1f)
        .setIgnoreTimeScale(true).setEaseInOutSine()
        .setOnUpdate(value => healthBar.color = value)
        .setOnComplete(() =>
        {
            LeanTween.value(healthBar.gameObject, Color.white, color, 0.1f)
            .setIgnoreTimeScale(true).setEaseInOutSine()
            .setOnUpdate(value => healthBar.color = value);
        });

        RectTransform rect = healthBar.rectTransform;
        Vector3 originalPosition = rect.anchoredPosition;

        LeanTween.move(rect, originalPosition + new Vector3(10f, 5f, 0f), 0.05f)
            .setIgnoreTimeScale(true).setLoopPingPong(5).setEaseInOutSine()
            .setOnComplete(() =>
            {
                healthBar.fillAmount = life / maxLife;
                rect.anchoredPosition = originalPosition;
            });
    }

    public void ChangeHealthBar(Color color)
    {
        LeanTween.value(gameObject, healthBar.color, color, 1f).setEaseInOutSine().setIgnoreTimeScale(true)
        .setOnUpdate(value => healthBar.color = value);
    }

    public void LostChance(float oldValue, float newValue)
    {
        deathPanel.gameObject.SetActive(true);
        RectTransform currentRect = chancesText.rectTransform;
        Vector2 centerPosition = currentRect.anchoredPosition;
        GameObject incomingObject = Instantiate(chancesText.gameObject, chancesText.transform.parent);
        TextMeshProUGUI incomingText = incomingObject.GetComponent<TextMeshProUGUI>();
        RectTransform incomingRect = incomingObject.GetComponent<RectTransform>();
        incomingText.text = newValue.ToString();
        incomingRect.anchorMin = currentRect.anchorMin;
        incomingRect.anchorMax = currentRect.anchorMax;
        incomingRect.pivot = currentRect.pivot;
        incomingRect.sizeDelta = currentRect.sizeDelta;
        incomingRect.anchoredPosition = centerPosition + Vector2.up * 60f;
        currentRect.anchoredPosition = centerPosition;

        LeanTween.cancel(currentRect.gameObject);
        LeanTween.cancel(incomingRect.gameObject);
        LeanTween.move(currentRect, centerPosition + Vector2.down * 60f, 3f).setEaseInCubic().setIgnoreTimeScale(true);
        LeanTween.move(incomingRect, centerPosition, 3f).setEaseOutCubic().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            Time.timeScale = 1f;
            chancesText.text = newValue.ToString();
            currentRect.anchoredPosition = centerPosition;
            Destroy(incomingObject);
            if (newValue <= 0)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
            deathPanel.gameObject.SetActive(false);
        });
    }


    // ----------- Private Methods -----------

    private IEnumerator I_BlackFade(Action onMiddle, Action onComplete)
    {
        LeanTween.cancel(panelFade.gameObject);

        float fadeDurationIn = fadeDuration / 4f;
        float fadeDurationOut = fadeDuration / 4f;
        float idleDuration = fadeDuration - fadeDurationIn - fadeDurationOut;

        bool tweenFinished = false;

        LeanTween.alpha(panelFade.rectTransform, 1f, fadeDurationIn).setIgnoreTimeScale(true).setOnComplete(() => tweenFinished = true);
        yield return new WaitUntil(() => tweenFinished);

        onMiddle?.Invoke();
        yield return new WaitForSecondsRealtime(idleDuration);

        tweenFinished = false;
        LeanTween.alpha(panelFade.rectTransform, 0f, fadeDurationOut).setIgnoreTimeScale(true).setOnComplete(() => tweenFinished = true);
        yield return new WaitUntil(() => tweenFinished);

        onComplete?.Invoke();
        fadeCoroutine = null;
    }

    
    // ----------- Visual Feedback Methods -----------


}
