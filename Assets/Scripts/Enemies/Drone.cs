using UnityEngine;

public class Drone : ChasePlayer
{
    private void Start()
    {
        TimeTravel.OnTimeChange.AddListener(SpinDrone);
    }

    private void OnDestroy()
    {
        TimeTravel.OnTimeChange.RemoveListener(SpinDrone);
    }
    void SpinDrone()
    {
        MusicManager.PlaySound(sound, gameObject.transform.position);
        isSpawning = true;
        LeanTween.rotateY(gameObject, 360f, 1f).setOnComplete(()=>
        {
            isSpawning = false;
        });
    }
}
