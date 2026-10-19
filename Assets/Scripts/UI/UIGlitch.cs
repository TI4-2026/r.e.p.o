using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIGlitch : MonoBehaviour
{
    [Header("Alvo")]
    [SerializeField] private RectTransform target;
    [SerializeField] private bool includeChildren = true;

    [Header("Intensidade")]
    [SerializeField] private float offset = 12f;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.9f, 1.1f);
    [SerializeField] private Vector2 sizeJitter = new Vector2(8f, 4f);
    [SerializeField] private bool affectSize = true;

    [Header("Timing")]
    [SerializeField] private float minInterval = 0.03f;
    [SerializeField] private float maxInterval = 0.15f;
    [SerializeField] private float duration = 0.6f;

    [Header("Cores do Glitch")]
    [SerializeField]
    private Color[] glitchColors = new Color[]
    {
        Color.pink,
        Color.cyan,
        Color.yellow,
        Color.purple,
        Color.white
    };
    [SerializeField] private float colorInterval = 0.06f;

    private class ChildState
    {
        public RectTransform rt;
        public Vector2 originalPos;
        public Vector3 originalScale;
        public Vector2 originalSize;
        public CanvasGroup group;
        public float originalAlpha;
        public Graphic graphic;
        public Color originalColor;
    }

    private readonly List<ChildState> states = new List<ChildState>();
    private Coroutine routine;
    private bool initialized;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        CacheChildren();
        initialized = true;
    }

    private void CacheChildren()
    {
        states.Clear();

        RectTransform[] all = includeChildren
            ? target.GetComponentsInChildren<RectTransform>(true)
            : new[] { target };

        foreach (var rt in all)
        {
            var state = new ChildState
            {
                rt = rt,
                originalPos = rt.anchoredPosition,
                originalScale = rt.localScale,
                originalSize = rt.sizeDelta,
                group = rt.GetComponent<CanvasGroup>(),
                graphic = rt.GetComponent<Graphic>()
            };

            if (state.group != null)
                state.originalAlpha = state.group.alpha;

            if (state.graphic != null)
                state.originalColor = state.graphic.color;

            states.Add(state);
        }
    }

    public void PlayGlitch()
    {
        if (!initialized) CacheChildren();

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Glitch(1));
    }

    public void StopGlitch()
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = null;

        RestoreAll();
    }

    public void ChangeGlitch()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Glitch(2));
    }

    private IEnumerator Glitch(int i)
    {
        float elapsed = 0f;
        float nextColorAt = 0f;

        while (elapsed < duration)
        {
            foreach (var s in states)
            {
                if (s.rt == null) continue;

                Vector2 off = new Vector2(
                    Random.Range(-offset*i, offset*i),
                    Random.Range(-offset * 0.15f, offset * 0.15f)
                );

                LeanTween.cancel(s.rt.gameObject);
                LeanTween.move(s.rt, s.originalPos + off, 0.03f)
                         .setEase(LeanTweenType.linear);

                s.rt.localScale = new Vector3(
                    s.originalScale.x * Random.Range(scaleRange.x*i, scaleRange.y*i),
                    s.originalScale.y * Random.Range(scaleRange.x*i, scaleRange.y*i),
                    s.originalScale.z
                );

                if (affectSize)
                {
                    s.rt.sizeDelta = new Vector2(
                        s.originalSize.x + Random.Range(-sizeJitter.x, sizeJitter.x),
                        s.originalSize.y + Random.Range(-sizeJitter.y, sizeJitter.y)
                    );
                }

                if (s.group != null)
                    s.group.alpha = Random.Range(0.5f, 1f);
            }

            if (Time.time >= nextColorAt)
            {
                nextColorAt = Time.time + colorInterval;
                ApplyRandomColors();
            }

            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);
            elapsed += wait;
        }

        RestoreAll();
        routine = null;
        if (i >= 2)
        {
            gameObject.SetActive(false);
        }
    }

    private void ApplyRandomColors()
    {
        if (glitchColors == null || glitchColors.Length == 0) return;

        foreach (var s in states)
        {
            if (s.graphic == null) continue;
            if (Random.value < 0.7f)
                s.graphic.color = glitchColors[Random.Range(0, glitchColors.Length)];
        }
    }

    private void RestoreAll()
    {
        foreach (var s in states)
        {
            if (s.rt == null) continue;

            LeanTween.cancel(s.rt.gameObject);

            s.rt.anchoredPosition = s.originalPos;
            s.rt.localScale = s.originalScale;
            s.rt.sizeDelta = s.originalSize;

            if (s.group != null)
                s.group.alpha = s.originalAlpha;

            if (s.graphic != null)
                s.graphic.color = s.originalColor;
        }
    }

    private void OnDisable()
    {
        StopGlitch();
    }
}