using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.InputSystem;
public class TimeTravel : MonoBehaviour
{
    public GameObject past;
    public GameObject future;
    public Slider cooldownSlider;
    float cooldown;
    public static bool isFuture = true;//se outro script for mudar o tempo, coloquem TimeTravel.isFuture = true/false
    public static Action<bool> OnTimeChange;//evento para chamar qualquer script que tenha mudanca de tempo
    void Start()
    {
        cooldownSlider.maxValue = 2f;
        cooldownSlider.minValue = 0f;
        cooldown = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownSlider.value = cooldown;
        if (cooldown < 2f)
        {
            cooldown += Time.deltaTime;
        }
    }

    public void ChangeTimeButton(InputAction.CallbackContext context)
    {
        if (context.performed && cooldown >= 2f)
        {
            StartCoroutine(ChangeTime(0.5f));
            cooldown = 0f;
        }
    }
    private IEnumerator ChangeTime(float delay)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delay);
        future.SetActive(!future.activeSelf);
        past.SetActive(!past.activeSelf);
        if (isFuture)
        {
            isFuture = false;
        }
        else
        {
            isFuture = true;
        }
        TimeChange(isFuture);
        Time.timeScale = 1f;
    }

    public static void TimeChange(bool isFuture)//fiz uma funcao estatica para caso algum outro script queira chamar(Ex:Ecilia)
    {
        OnTimeChange?.Invoke(isFuture);
    }
}
