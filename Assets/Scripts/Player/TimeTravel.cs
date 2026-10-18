using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.InputSystem;
public class TimeTravel : MonoBehaviour
{
    public GameObject Alicefuturo;
    public GameObject Alicepassado;
    public Animator PlayerAnim;
    public Avatar AlicePassado;
    public Avatar AliceFuturo;
    public GameObject past;
    public GameObject future;
    public Slider cooldownSlider;
    public Material skyboxfuturo;
    public Material skyboxpassado;

    [Header("SFX")]
    [SerializeField] private AudioClip sound;

    float cooldown;
    public static bool isFuture = true;//se outro script for mudar o tempo, coloquem TimeTravel.isFuture = true/false
    public static Action<bool> OnTimeChange;//evento para chamar qualquer script que tenha mudanca de tempo
    void Start()
    {
        cooldownSlider.maxValue = 2f;
        cooldownSlider.minValue = 0f;
        cooldown = 2f;
        RenderSettings.skybox = skyboxfuturo;
        Alicefuturo.SetActive(false);
        Alicepassado.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        cooldownSlider.value = cooldown;
        if (cooldown < 2f)
        {
            cooldown += Time.unscaledDeltaTime;
        }
    }

    public void ChangeTimeButton(InputAction.CallbackContext context)
    {
        if (context.performed && cooldown >= 2f)
        {
            MusicManager.PlaySound(sound, gameObject.transform.position);
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
            RenderSettings.skybox = skyboxpassado;
            PlayerAnim.avatar = AliceFuturo;
            Alicepassado.SetActive(false);
            Alicefuturo.SetActive(true);
        }
        else
        {
            isFuture = true;
            RenderSettings.skybox = skyboxfuturo;
            PlayerAnim.avatar = AlicePassado;
            Alicefuturo.SetActive(false);
            Alicepassado.SetActive(true);
        }
        TimeChange(isFuture);
        Time.timeScale = 1f;
    }

    public static void TimeChange(bool isFuture)//fiz uma funcao estatica para caso algum outro script queira chamar(Ex:Ecilia)
    {
        OnTimeChange?.Invoke(isFuture);
    }
}
