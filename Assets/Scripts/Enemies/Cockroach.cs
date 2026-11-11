using UnityEngine;

public class Cockroach : ChasePlayer
{
    [Header("Barata")]
    [SerializeField] private float jumpForce = 5f;

    private void Start()
    {
        TimeTravel.OnTimeChange.AddListener(ScaredRoach);
    }

    private void OnDestroy()
    {
        TimeTravel.OnTimeChange.RemoveListener(ScaredRoach);
    }
    public void ScaredRoach()
    {
        MusicManager.PlaySound(sound, gameObject.transform.position);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
