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
        MusicManager.PlaySound(sound, gameObject.transform.position);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
