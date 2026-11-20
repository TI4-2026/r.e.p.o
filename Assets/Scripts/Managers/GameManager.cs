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

    public void SetKey(ItemBehavior key)
    {
        keyLevel = key;

        DialogData currentDialog = firstKeyDialog;
        switch (keysCollected)
        {
            case 0:
                currentDialog = firstKeyDialog;
                break;
            case 1:
                currentDialog = secondKeyDialog;
                break;
            case 2:
                currentDialog = thirdKeyDialog;
                break;
        }
        key.SetDialog(currentDialog);
    }
    
    public void CollectKey(bool enableExit = false)
    {
        keysCollected++;
        if (enableExit) EnableExit();
    }
    
    public void SpawnPlayer(GameObject player, bool shownBlackFade = false)
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
                }
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
        SpawnPlayer(player, showBlackFade);
        
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
