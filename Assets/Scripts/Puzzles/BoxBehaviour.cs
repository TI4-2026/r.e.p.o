using UnityEngine;

public class BoxBehaviour : PlayerCollisionObject // !!! ESTA HERDANDO DE "PlayerCollisionObject" !!!
{
    [SerializeField] private bool freezePast; //Define se o objeto vai ser congelado no passado
    [SerializeField] private bool freezeFuture; //Define se o objeto vai ser congelado no futuro

    private Rigidbody rb;
    private PlayerInteraction playerInteraction;
    private bool isMoveObjectMode = false;
    private bool isPlayerClose = false;
    private bool falling = false;
    private Transform defaultParent;

    // ----------------- Unity Methods -----------------

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        defaultParent = gameObject.transform.parent;
    }

    private void Update() {

        if (!isMoveObjectMode) return;

        if (rb.linearVelocity.y < -0.5f)
        {
            playerInteraction?.TryMoveObject();
        }
    }

    // ----------------- Public Methods -----------------

    public override void OnPlayerCollisionEnter(GameObject player)
    {
        if (IsDisableInteraction() || falling) return;
        
        isPlayerClose = true;
        playerInteraction = player.GetComponent<PlayerInteraction>();
        playerInteraction.PossibilityToMoveObject(gameObject, EnableMoveObjectMode, DisableMoveObjectMode);
    }

    public override void OnPlayerCollisionExit(GameObject player)
    {
        if (!isPlayerClose) return;

        isPlayerClose = false;
        playerInteraction.RemovePossibilityToMoveObject(gameObject);
        playerInteraction = null;
    }

    // ----------------- Private Methods -----------------

    public void EnableMoveObjectMode()
    {
        isMoveObjectMode = true;
        gameObject.transform.parent = playerInteraction.gameObject.transform;
    }

    public void DisableMoveObjectMode()
    {
        isMoveObjectMode = false;
        gameObject.transform.parent = defaultParent;
    }

    private bool IsDisableInteraction()
    {
        if (freezePast && !TimeTravel.isFuture) return true;
        if (freezeFuture && TimeTravel.isFuture) return true;
        return false;
    }
}
