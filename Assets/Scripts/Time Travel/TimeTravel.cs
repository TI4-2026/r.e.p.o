using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class TimeTravel : MonoBehaviour
{
    // ------------- Singleton -------------

    public static TimeTravel Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ---------------------------------------


    [SerializeField] TimeTravelInfo futureSO;
    [SerializeField] TimeTravelInfo pastSO;

    public Animator PlayerAnim;

    [Header("SFX")]
    [SerializeField] private AudioClip sound;

    [Header("Transition")]
    [SerializeField, Min(0.1f)] private float transitionFadeDuration = 0.6f;

    private float cooldown;
    private bool isChangingTime;
    private const float CooldownDuration = 2f;

    public static bool isFuture = true;//se outro script for mudar o tempo, coloquem TimeTravel.isFuture = true/false
    public static Action<bool> OnTimeChange;//evento para chamar qualquer script que tenha mudanca de tempo
    
    void Start()
    {
        cooldown = 2f;
        UpdateCooldownDisplay();
        ApplyTimeInfo(isFuture ? futureSO : pastSO);
    }

    void Update()
    {
        cooldown += Time.deltaTime;
        UpdateCooldownDisplay();
    }

    public void ChangeTimeButton(InputAction.CallbackContext context)
    {
        if (context.performed && cooldown >= CooldownDuration && !isChangingTime)
        {
            MusicManager.PlaySound(sound, gameObject.transform.position);
            ChangeTime();
            cooldown = 0f;
        }
    }

    private void ChangeTime()
    {
        isChangingTime = true;
        Time.timeScale = 0f;

        void ApplyTimeChange()
        {
            isFuture = !isFuture;
            ApplyTimeInfo(isFuture ? futureSO : pastSO);
            TimeChange(isFuture);
        }

        void FinishTransition()
        {
            Time.timeScale = 1f;
            isChangingTime = false;
        }

        GameManager.Instance.Hud.BlackFade(ApplyTimeChange, FinishTransition, transitionFadeDuration);
    }

    private void ApplyTimeInfo(TimeTravelInfo timeInfo)
    {
        futureSO.mapCollection.SetActive(timeInfo == futureSO);
        pastSO.mapCollection.SetActive(timeInfo == pastSO);
        futureSO.playerModel.SetActive(timeInfo == futureSO);
        pastSO.playerModel.SetActive(timeInfo == pastSO);
        PlayerAnim.avatar = timeInfo.playerAvatar;
        RenderSettings.skybox = timeInfo.skybox;
    }

    private void UpdateCooldownDisplay()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.Hud.SetTimeTravelCooldown(cooldown, CooldownDuration);
    }

    //fiz uma funcao estatica para caso algum outro script queira chamar (Ex:Ecilia)
    public static void TimeChange(bool isFuture)
    {
        OnTimeChange?.Invoke(isFuture);
    }
}
