using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
public class TimeTravel : MonoBehaviour
{
    // ------------- Singleton -------------

    public static TimeTravel Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isFuture = true;
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            OnTimeChange.RemoveAllListeners();
        }
    }

    // ---------------------------------------
    
    [SerializeField] TimeTravel_ManagerInfo futureSO;
    [SerializeField] TimeTravel_ManagerInfo pastSO;
    
    public static bool isFuture = true;
    public static UnityEvent OnTimeChange = new UnityEvent();
    
    [Header("Settings")]
    [SerializeField, Min(0.1f)] private float transitionFadeDuration = 0.6f;

    [Header("Others")]
    public Animator PlayerAnim;
    
    private float cooldown;
    private bool isChangingTime;
    private const float CooldownDuration = 2f;
    
    private PlayerCollisionSelf playerCollisionSelf;
    private GameManager gameManager;
    
    // ----------------- Default Methods -----------------
    
    void Start()
    {
        cooldown = 2f;
    
        playerCollisionSelf = GameManager.Instance.player.GetComponent<PlayerCollisionSelf>();
        gameManager = GameManager.Instance;
        
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
        if (Time.timeScale == 0f) return;
        
        if (gameManager.IsRespawning) return;
        if (gameManager.Hud != null && (gameManager.Hud.IsFading || gameManager.Hud.ActiveFade)) return;

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

        Time.timeScale = 0;
        GameManager.Instance.Hud.TimeChangeEffect();
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
        Hud hud = GameManager.Instance != null ? GameManager.Instance.Hud : null;
        if (hud == null || !hud.ActiveFade)
        {
            Time.timeScale = 1f;
        }
    }

    private void ApplyTimeChangeAux(TimeTravel_ManagerInfo timeManagerInfo, bool toEnable, bool checkCollisions)
    {
        timeManagerInfo.mapCollection.SetActive(toEnable);
        timeManagerInfo.playerModel.SetActive(toEnable);

        if (toEnable)
        {
            RenderSettings.skybox = timeManagerInfo.skybox;
        }

        if (checkCollisions) playerCollisionSelf.CheckPlayerInsideObject(restartPlayer: true);
    }

    private void UpdateCooldownDisplay()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.Hud.SetTimeTravelCooldown(cooldown, CooldownDuration);
    }
}
