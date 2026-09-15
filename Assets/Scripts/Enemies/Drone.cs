using UnityEngine;

public class Drone : ChasePlayer
{
    private void Start()
    {
        TimeTravel.OnTimeChange += SpinDrone;
    }

    private void OnDestroy()
    {
        TimeTravel.OnTimeChange -= SpinDrone;
    }
    void SpinDrone(bool timeChange)
    {
        isSpawning = true;
        LeanTween.rotateY(gameObject, 360f, 1f).setOnComplete(()=>
        {
            isSpawning = false;
        });
    }
}
