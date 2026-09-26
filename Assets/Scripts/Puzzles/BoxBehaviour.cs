using UnityEngine;

public class BoxBehaviour : PlayerCollisionObject // !!! ESTA HERDANDO DE "PlayerCollisionObject" !!!
{
    [SerializeField] private bool freezePast; //Define se o objeto vai ser congelado no passado
    [SerializeField] private bool freezeFuture; //Define se o objeto vai ser congelado no futuro

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public override void OnPlayerCollisionEnter(GameObject player)
    {
        Debug.Log("BoxBehaviour: OnPlayerCollisionEnter");
    }

    public override void OnPlayerCollisionExit(GameObject player)
    {
        Debug.Log("BoxBehaviour: OnPlayerCollisionExit");
    }
}
