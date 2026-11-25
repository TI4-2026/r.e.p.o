using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private Gradient gradient;

    [Header("References")]
    [SerializeField] private Image panelFade;
    public Image healthBar;
    [SerializeField] private Animator timeTravelEffect;
    [SerializeField] private AudioClip timeSoundSfx;
    [SerializeField] private Slider timeTravelCooldownSlider;
    [SerializeField] private Image timeTravelFill;
    [SerializeField] private Image timeTravelBackground;
    [SerializeField] Color timeTravelDisabledColor;
    private Color _timeTravelDefaultColor;

    private Coroutine fadeCoroutine;
    private Action activeFadeOnComplete;

    public bool IsFading => fadeCoroutine != null;

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
        panelFade.gameObject.SetActive(false);
        
        _timeTravelDefaultColor = timeTravelFill.color;
        
        timeTravelEffect.gameObject.SetActive(false);
    }

    // ----------- Public Methods -----------

    public void BlackFade(Action onMiddle = null, Action onComplete = null, float duration = -1f)
    {
        Action previousOnComplete = activeFadeOnComplete;
        Action allOnComplete = previousOnComplete != null
            ? () => { previousOnComplete.Invoke(); onComplete?.Invoke(); }
            : onComplete;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        activeFadeOnComplete = allOnComplete;
        fadeCoroutine = StartCoroutine(I_BlackFade(onMiddle, allOnComplete, duration > 0f ? duration : fadeDuration));
    }
    
    public void BlackFadeFinishOnMiddle(Action onComplete = null, float duration = -1f)
    {
        Action previousOnComplete = activeFadeOnComplete;
        Action allOnComplete = previousOnComplete != null
            ? () => { previousOnComplete.Invoke(); onComplete?.Invoke(); }
            : onComplete;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        activeFadeOnComplete = allOnComplete;
        fadeCoroutine = StartCoroutine(I_BlackFadeFinishOnMiddle(allOnComplete, duration > 0f ? duration : fadeDuration));
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

    public void ChangeHealthBar(float life, float maxLife)
    {
        LeanTween.value(gameObject, healthBar.color, gradient.Evaluate(life/maxLife), 1f).setEaseInOutSine().setIgnoreTimeScale(true)
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

    public void EnableTimeTravelSlider()
    {
        timeTravelBackground.color = _timeTravelDefaultColor;
        timeTravelFill.color = _timeTravelDefaultColor;
    }

    public void DisableTimeTravelSlider()
    {
        timeTravelBackground.color = timeTravelDisabledColor;
        timeTravelFill.color = timeTravelDisabledColor;
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

        try
        {
            onMiddle?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        yield return new WaitForSecondsRealtime(idleDuration);

        tweenFinished = false;
        LeanTween.alpha(panelFade.rectTransform, 0f, fadeDurationOut).setIgnoreTimeScale(true).setOnComplete(() => tweenFinished = true);
        yield return new WaitUntil(() => tweenFinished);

        fadeCoroutine = null;
        activeFadeOnComplete = null;
        panelFade.gameObject.SetActive(false);
        onComplete?.Invoke();
    }
    
    private IEnumerator I_BlackFadeFinishOnMiddle(Action onComplete, float totalDuration)
    {
        panelFade.gameObject.SetActive(true);
        LeanTween.cancel(panelFade.gameObject);

        bool tweenFinished = false;

        LeanTween.alpha(panelFade.rectTransform, 1f, totalDuration)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => tweenFinished = true);
        
        yield return new WaitUntil(() => tweenFinished);
        fadeCoroutine = null;
        activeFadeOnComplete = null;
        onComplete?.Invoke();
    }
    
    public void TimeChangeEffect(float animSpeedMulti=1f, bool showEndFewSeconds = true)
    {
        MusicManager.PlaySound(timeSoundSfx, transform.position);
        timeTravelEffect.gameObject.SetActive(true);
        timeTravelEffect.SetFloat("Speed", 1f * animSpeedMulti);
        timeTravelEffect.Play("ClockAnim");
        StartCoroutine(DeactivateClock(animSpeedMulti, showEndFewSeconds));
    }

    IEnumerator DeactivateClock(float animSpeedMulti=1f, bool showEndFewSeconds = true)
    {
        yield return new WaitForSecondsRealtime(0.5f / animSpeedMulti);
        timeTravelEffect.SetFloat("Speed", -2 * animSpeedMulti);
        timeTravelEffect.Play("ClockAnim");
        if (showEndFewSeconds) yield return new WaitForSecondsRealtime(0.125f);
        timeTravelEffect.gameObject.SetActive(false);
    }
}
