using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    // --------------- Singleton ---------------
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (Hud == null)
        {
            Hud = FindFirstObjectByType<Hud>();
        }
    }

    // --------------- Variables ---------------

    [Header("References")]
    [SerializeField] private Transform spawn;
    [SerializeField] private DialogData firstKeyDialog;
    [SerializeField] private DialogData secondKeyDialog;
    [SerializeField] private DialogData thirdKeyDialog;

    [Header("Autofilled")]
    public Hud Hud;
    public Transform checkpoint = null;
    private TransitionSceneTrigger exitLevel = null;
    public GameObject player;
    

    private ItemBehavior keyLevel;
    private int keysCollected = 0;
    private int currentCheckpointPriority = 0;
    public bool IsRespawning { get; private set; }

    // --------------- Public Methods ---------------
    
    public void CollectKey(bool enableExit = false)
    {
        Debug.Log(keysCollected);
        keysCollected++;
        if (enableExit) EnableExit();
    }

    public void StartKeyDialog()
    {
        DialogData currentDialog = firstKeyDialog;
        switch (keysCollected)
        {
            case 1:
                currentDialog = firstKeyDialog;
                break;
            case 2:
                currentDialog = secondKeyDialog;
                break;
            case 3:
                currentDialog = thirdKeyDialog;
                break;
            case 4:
                return;
        }
        DialogManager.Instance.IniciarDialogo(currentDialog);
    }
    
    public void RespawnPlayer(GameObject player, bool shownBlackFade = false, float duration=1f)
    {
        Transform teleportPoint = checkpoint != null ? checkpoint : spawn;
        player.TryGetComponent(out PlayerMovement playerMovement);

        if (IsRespawning) return;

        if (shownBlackFade)
        {
            IsRespawning = true;
            playerMovement.SetMovementEnabled(false);

            Hud.BlackFade(
                onMiddle: () =>
                {
                    playerMovement.SetMovementEnabled(false);
                    playerMovement.ExecuteTeleport(teleportPoint);
                },
                onComplete: () =>
                {
                    IsRespawning = false;
                    playerMovement.SetMovementEnabled(true);
                },
                duration
            );
        }
        else
        {
            playerMovement.ExecuteTeleport(teleportPoint);
            playerMovement.SetMovementEnabled(true);
        }
    }

    public void KillPlayer(GameObject player, bool showBlackFade = false)
    {
        RespawnPlayer(player, showBlackFade, 0.75f);
        
    }

    public void SetCheckpoint(Transform newCheckpoint, int priority=0)
    {
        if (priority < currentCheckpointPriority) return;

        checkpoint = newCheckpoint;
        currentCheckpointPriority = priority;
    }

    public void SetExit(TransitionSceneTrigger exit)
    {
        exitLevel = exit;
    }
    
    // --------------- Private Methods ---------------
    
    private void EnableExit()
    {
        if (exitLevel == null) return;
        exitLevel.SetEnabled(true);
    }
}
