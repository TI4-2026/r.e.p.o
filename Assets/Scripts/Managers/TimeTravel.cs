using UnityEngine;
using System;
using UnityEngine.Events;
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
    
    public static bool isFuture = true;
    public static UnityEvent OnTimeChange = new UnityEvent();
    
    [Header("Settings")]
    [SerializeField, Min(0.1f)] private float transitionFadeDuration = 0.6f;

    [Header("Others")]
    [SerializeField] private AudioClip sound;
    public Animator PlayerAnim;

        
    private float cooldown;
    private bool isChangingTime;
    private const float CooldownDuration = 2f;
    
    private PlayerCollisionSelf playerCollisionSelf;
    
    // ----------------- Default Methods -----------------
    
    void Start()
    {
        cooldown = 2f;
    
        playerCollisionSelf = GameManager.Instance.player.GetComponent<PlayerCollisionSelf>();
        
        UpdateCooldownDisplay();
        ApplyTimeChange();
    }

    void Update()
    {
        cooldown += Time.deltaTime;
        UpdateCooldownDisplay();
    }
    
    // ----------------- Public Methods -----------------

    public void OnChangeTime()
    {
        if (cooldown >= CooldownDuration && !isChangingTime)
        {
            ChangeTime();
            cooldown = 0f;
        }
    }
    
    // ----------------- Private Methods -----------------
    
    private void ChangeTime()
    {
        isChangingTime = true;
        isFuture = !isFuture;
        MusicManager.PlaySound(sound, gameObject.transform.position);

        Time.timeScale = 0;
        GameManager.Instance.Hud.BlackFade(
        ApplyTimeChange, 
        FinishTimeChange,
        transitionFadeDuration);
    }

    private void ApplyTimeChange()
    {
        OnTimeChange.Invoke();
        if (isFuture)
        {
            ApplyTimeChangeAux(futureSO, true, true);
            ApplyTimeChangeAux(pastSO, false, false);
        }
        else
        {
            ApplyTimeChangeAux(pastSO, true, true);
            ApplyTimeChangeAux(futureSO, false, false);
        }
    }

    private void FinishTimeChange()
    {
        isChangingTime = false;
        Time.timeScale = 1;
    }

    private void ApplyTimeChangeAux(TimeTravelInfo timeInfo, bool toEnable, bool checkCollisions)
    {
        timeInfo.mapCollection.SetActive(toEnable);
        timeInfo.playerModel.SetActive(toEnable);

        if (toEnable)
        {
            RenderSettings.skybox = timeInfo.skybox;
        }

        if (checkCollisions) playerCollisionSelf.CheckPlayerInsideObject(diePlayer: true);
    }

    private void UpdateCooldownDisplay()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.Hud.SetTimeTravelCooldown(cooldown, CooldownDuration);
    }
}
