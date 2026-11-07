using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    // -------------- Singleton --------------
    public static InfoManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    
    [Header("References")]

    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Image tipUnderbar;

    private bool isTipActive = false;
    private LTDescr currentTween;
    private Coroutine hideCoroutine;

    private Color tipTextColor;
    private Color tipUnderbarColor;
    
    // -------------- Default Methods --------------
    
    /*
     * Para funcionar o sistema de informação, um trigger com InfoTrigger.cs deve ser colocado no local desejado, com o texto e a duração da informação.
     */
    
    private void Start()
    {
        tipTextColor = tipText.color;
        tipUnderbarColor = tipUnderbar.color;

        tipTextColor.a = 0f;
        tipUnderbarColor.a = 0f;

        tipText.color = tipTextColor;
        tipUnderbar.color = tipUnderbarColor;
    }
    
    // -------------- Public Methods --------------
    
    public void CreateTip(string text, float duration = 5f)
    {
        CancelTipAnimation();
        tipText.text = text;
        isTipActive = true;
        
        currentTween = LeanTween.value(0f, 1f, 0.5f)
            .setOnUpdate((float value) =>
            {
                tipTextColor.a = value;
                tipUnderbarColor.a = value;
                tipUnderbar.color = tipUnderbarColor;
                tipText.color = tipTextColor;
            })
            .setOnComplete(() => hideCoroutine = StartCoroutine(I_HideTip(duration)));
    }

    private IEnumerator I_HideTip(float duration)
    {
        yield return new WaitForSeconds(duration);
        hideCoroutine = null;

        if (isTipActive)
        {
            currentTween = LeanTween.value(1f, 0f, 0.5f)
                .setOnUpdate((float value) =>
                {
                    tipTextColor.a = value;
                    tipUnderbarColor.a = value;
                    tipUnderbar.color = tipUnderbarColor;
                    tipText.color = tipTextColor;
                })
                .setOnComplete(() => isTipActive = false);
        }
    }

    private void CancelTipAnimation()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (currentTween != null)
        {
            LeanTween.cancel(currentTween.id);
            currentTween = null;
        }
        isTipActive = false;

        tipTextColor.a = 0f;
        tipUnderbarColor.a = 0f;
        tipText.color = tipTextColor;
        tipUnderbar.color = tipUnderbarColor;
    }
}
