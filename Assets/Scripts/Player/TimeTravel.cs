using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class TimeTravel : MonoBehaviour
{
    public GameObject past;
    public GameObject future;
    public Slider cooldownSlider;
    float cooldown;
    public static bool isFuture = true;//se outro script for mudar o tempo, coloquem TimeTravel.isFuture = true/false
    public static Action<bool> OnTimeChange;//evento para chamar qualquer script que tenha mudança de tempo
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
        if (Input.GetKeyDown(KeyCode.E) && cooldown >= 2f)
        {
            StartCoroutine(ChangeTime(0.5f)); 
            cooldown = 0f;
        }
    }
    private IEnumerator ChangeTime(float delay)
    {
        yield return new WaitForSeconds(delay);
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
    }

    public static void TimeChange(bool isFuture)//fiz uma função estática para caso algum outro script queira chamar(Ex:Ecilia)
    {
        OnTimeChange?.Invoke(isFuture);
    }
}
