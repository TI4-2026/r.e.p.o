using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using Random = System.Random;

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
    
    public static bool isFuture = true;
    public static UnityEvent OnTimeChange = new UnityEvent();
    
    [Header("References")]
    
    [Tooltip("Script que contem informacoes do futuro")]
    [SerializeField] TimeTravel_ManagerInfo futureSO;
    
    [Tooltip("Script que contem informacoes do passado")]
    [SerializeField] TimeTravel_ManagerInfo pastSO;
    
    [Tooltip("Dialogo da primeira vez que o player morrer por colisao ao mudar de tempo")]
    [SerializeField] private DialogData deathDialogInitial;
    
    [Tooltip("Dialogos aleatorios que aparecerao quando o player morrer por colisao ao mudar de tempo")]
    [SerializeField] private DialogData[] deathDialogs;
    
    [Header("Others")]
    
    [SerializeField, Min(0.1f)] private float transitionFadeDuration = 0.6f;
    [SerializeField] private Animator PlayerAnim;
    
    private float cooldown;
    private bool isChangingTime;
    private const float CooldownDuration = 2f;
    private int playersCollisionDeath; // Quant. vezes o player morreu por colisao dentro de objeto
    
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
        if (gameManager.Hud.IsFading || gameManager.Hud.ActiveFade) return;
        if (isChangingTime) return;

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
    
    private void ApplyTimeChangeAux(TimeTravel_ManagerInfo timeManagerInfo, bool toEnable, bool checkCollisions)
    {
        timeManagerInfo.mapCollection.SetActive(toEnable);
        timeManagerInfo.playerModel.SetActive(toEnable);

        if (toEnable)
        {
            RenderSettings.skybox = timeManagerInfo.skybox;
        }

        if (checkCollisions)
        {
            if (playerCollisionSelf.CheckPlayerInsideObject(restartPlayer: true, showBlackFade: false))
            {
                playersCollisionDeath++;
                
                switch (playersCollisionDeath)
                {
                    case 1:
                        DialogManager.Instance.IniciarDialogo(deathDialogInitial);
                        break;
                    default:
                        int dialogSelected = UnityEngine.Random.Range(0, deathDialogs.Length);
                        DialogManager.Instance.IniciarDialogo(deathDialogs[dialogSelected]);
                        break;
                }
            }
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

    private void UpdateCooldownDisplay()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.Hud.SetTimeTravelCooldown(cooldown, CooldownDuration);
    }
}
