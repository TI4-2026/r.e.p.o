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
        isSpawning = true;
        LeanTween.rotateY(gameObject, 360f, 1f).setOnComplete(()=>
        {
            isSpawning = false;
        });
    }
}
