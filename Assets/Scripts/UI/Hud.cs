using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private Color fadeColor = Color.black;

    [Header("References")]
    [SerializeField] private Image panelFade;

    private Coroutine fadeCoroutine;
    
    private void Awake() {
        if (GameManager.Instance != null) GameManager.Instance.Hud = this;
    }

    private void Start()
    {
        panelFade.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
    }

    // ----------- Public Methods -----------

    public void BlackFade(Action onMiddle = null, Action onComplete = null)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        
        fadeCoroutine = StartCoroutine(FadeRoutine(onMiddle, onComplete));
    }

    private IEnumerator FadeRoutine(Action onMiddle, Action onComplete)
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
}
