using System;
using System.Collections;
using TMPro;
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
    [SerializeField] private Animator timeTravelEffect;
    [SerializeField] private Slider timeTravelCooldownSlider;

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
        TimeTravel.OnTimeChange += TimeChangeEffect;
        timeTravelEffect.gameObject.SetActive(false);
        panelFade.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        panelFade.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        TimeTravel.OnTimeChange -= TimeChangeEffect;   
    }

    // ----------- Public Methods -----------

    public void BlackFade(Action onMiddle = null, Action onComplete = null, float duration = -1f)
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
        
        fadeCoroutine = StartCoroutine(I_BlackFade(onMiddle, onComplete, duration > 0f ? duration : fadeDuration));
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

    public void SetTimeTravelCooldown(float value, float maxValue)
    {
        if (timeTravelCooldownSlider == null) return;

        if (!Mathf.Approximately(timeTravelCooldownSlider.minValue, 0f))
        {
            timeTravelCooldownSlider.minValue = 0f;
        }

        if (!Mathf.Approximately(timeTravelCooldownSlider.maxValue, maxValue))
        {
            timeTravelCooldownSlider.maxValue = maxValue;
        }

        timeTravelCooldownSlider.value = value;
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
        LeanTween.move(currentRect, centerPosition + Vector2.down * 500f, 3f).setEaseInCubic().setIgnoreTimeScale(true);
        LeanTween.move(incomingRect, centerPosition, 3f).setEaseOutCubic().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            healthBar.color = Color.green;
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

    private IEnumerator I_BlackFade(Action onMiddle, Action onComplete, float totalDuration)
    {
        panelFade.gameObject.SetActive(true);
        LeanTween.cancel(panelFade.gameObject);

        float fadeDurationIn = totalDuration / 4f;
        float fadeDurationOut = totalDuration / 4f;
        float idleDuration = totalDuration - fadeDurationIn - fadeDurationOut;

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
        panelFade.gameObject.SetActive(false);
    }

    
    // ----------- Visual Feedback Methods -----------

    private void TimeChangeEffect(bool time)
    {
        timeTravelEffect.gameObject.SetActive(true);
        timeTravelEffect.SetFloat("Speed", 0.5f);
        timeTravelEffect.Play("ClockAnim");
        StartCoroutine(DeactivateClock());
    }

    IEnumerator DeactivateClock()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        timeTravelEffect.SetFloat("Speed", -1);
        timeTravelEffect.Play("ClockAnim");
        yield return new WaitForSecondsRealtime(0.5f);
        timeTravelEffect.gameObject.SetActive(false);
    }
}
