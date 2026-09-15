using UnityEngine;

public class Cockroach : ChasePlayer
{
    [Header("Barata")]
    [SerializeField] private float jumpForce = 5f;

    private void Start()
    {
        TimeTravel.OnTimeChange += ScaredRoach;   
    }

    private void OnDestroy()
    {
        TimeTravel.OnTimeChange -= ScaredRoach;
    }
    public void ScaredRoach(bool isFuture)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
