using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private Color fadeColor = Color.black;

    [Header("References")]
    [SerializeField] private Image panelFade;
    [SerializeField] private Image healthBar;

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

    // ----------- Private Methods -----------

    private IEnumerator I_BlackFade(Action onMiddle, Action onComplete)
    {
        LeanTween.cancel(panelFade.gameObject);

        float fadeDurationIn = fadeDuration / 4f;
        float fadeDurationOut = fadeDuration / 4f;
        float idleDuration = fadeDuration - fadeDurationIn - fadeDurationOut;

        bool tweenFinished = false;

        LeanTween.alpha(panelFade.rectTransform, 1f, fadeDurationIn).setOnComplete(() => tweenFinished = true);
        yield return new WaitUntil(() => tweenFinished);

        onMiddle?.Invoke();
        yield return new WaitForSeconds(idleDuration);

        tweenFinished = false;
        LeanTween.alpha(panelFade.rectTransform, 0f, fadeDurationOut).setOnComplete(() => tweenFinished = true);
        yield return new WaitUntil(() => tweenFinished);

        onComplete?.Invoke();
        fadeCoroutine = null;
    }

    /*
    // ----------- Visual Feedback Methods -----------

    private Image GetFillImage()
    {
        if (healthFillImage != null)
            return healthFillImage;

        if (healthSlider != null && healthSlider.fillRect != null)
        {
            healthFillImage = healthSlider.fillRect.GetComponent<Image>();
            return healthFillImage;
        }

        return null;
    }

    private void FlashHealthBar()
    {
        Image fill = GetFillImage();
        if (fill == null) return;

        Color originalColor = fill.color;
        LeanTween.cancel(fill.gameObject);
        LeanTween.value(fill.gameObject, originalColor, Color.white, 0.1f)
            .setIgnoreTimeScale(true).setEaseInOutSine()
            .setOnUpdate(val => fill.color = val)
            .setOnComplete(() =>
            {
                LeanTween.value(fill.gameObject, Color.white, originalColor, 0.1f)
                    .setIgnoreTimeScale(true).setEaseInOutSine()
                    .setOnUpdate(val => fill.color = val);
            });
    }

    private void ShakeHealthBar()
    {
        if (healthSlider == null) return;

        RectTransform rect = healthSlider.GetComponent<RectTransform>();
        if (rect == null) return;

        Vector3 originalPosition = rect.anchoredPosition;
        LeanTween.cancel(rect.gameObject);
        LeanTween.move(rect, originalPosition + new Vector3(10f, 5f, 0f), 0.05f)
            .setIgnoreTimeScale(true).setLoopPingPong(5).setEaseInOutSine()
            .setOnComplete(() =>
            {
                rect.anchoredPosition = originalPosition;
            });
    }

    private void UpdateHealthVisuals(float currentHealth, float maxHealth)
    {
        if (!useColorProgression || maxHealth <= 0f) return;

        Image fill = GetFillImage();
        if (fill == null) return;

        if (currentHealth <= maxHealth / 2f && currentHealth > maxHealth / 4f && healthProgression == 0)
        {
            healthProgression = 1;
            ChangeHealthBarColor(mediumHealthColor);
        }
        else if (currentHealth <= maxHealth / 4f && currentHealth > 0f && healthProgression <= 1)
        {
            healthProgression = 2;
            ChangeHealthBarColor(lowHealthColor);
        }
        else if (currentHealth > maxHealth / 2f && healthProgression != 0)
        {
            healthProgression = 0;
            ChangeHealthBarColor(fullHealthColor);
        }
    }

    private void ChangeHealthBarColor(Color targetColor)
    {
        Image fill = GetFillImage();
        if (fill == null) return;

        LeanTween.cancel(fill.gameObject);
        LeanTween.value(fill.gameObject, fill.color, targetColor, 0.5f)
            .setEaseInOutSine().setIgnoreTimeScale(true)
            .setOnUpdate(val => fill.color = val);
    }
    */
}
