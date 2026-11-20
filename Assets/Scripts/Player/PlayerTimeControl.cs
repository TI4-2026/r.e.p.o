
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerTimeControl : MonoBehaviour
{
    [Header("Time Stop")]
    [SerializeField] private float tempoTimeStop = 5f;
    [SerializeField] private float cooldown = 10f;

    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Global Volume")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile normal;
    [SerializeField] private VolumeProfile timeStop;

    private TimeManager timeManager;

    private bool podeUsarTimeStop = true;

    private Coroutine rotinaAtual;

    void Start()
    {
        timeManager = TimeManager.Instance;

        slider.maxValue = cooldown;
        slider.value = cooldown;

        globalVolume.sharedProfile = normal;
    }

    public void TimeStop(InputAction.CallbackContext context)
    {
        if (context.performed && podeUsarTimeStop)
        {
            Debug.Log("Time Stop");

            podeUsarTimeStop = false;

            timeManager.SetTimeMultiplier(0f);

            globalVolume.sharedProfile = timeStop;

            rotinaAtual = StartCoroutine(ChangeBack());
        }
    }

    IEnumerator ChangeBack()
    {
        float tempoAtual = tempoTimeStop;

        while (tempoAtual > 0f)
        {
            tempoAtual -= Time.deltaTime;

            slider.value = Mathf.Lerp(
                0f,
                cooldown,
                tempoAtual / tempoTimeStop
            );

            yield return null;
        }

        slider.value = 0f;

        timeManager.SetTimeMultiplier(1f);

        globalVolume.sharedProfile = normal;

        yield return StartCoroutine(Cooldown());

        podeUsarTimeStop = true;
        rotinaAtual = null;
    }

    IEnumerator Cooldown()
    {
        float tempoAtual = 0f;

        while (tempoAtual < cooldown)
        {
            tempoAtual += Time.deltaTime;

            slider.value = Mathf.Lerp(
                0f,
                cooldown,
                tempoAtual / cooldown
            );

            yield return null;
        }

        slider.value = cooldown;
    }
}
