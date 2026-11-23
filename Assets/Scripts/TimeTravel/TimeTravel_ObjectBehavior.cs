using UnityEngine;

public class TimeTravel_ObjectBehavior : MonoBehaviour
{
    [SerializeField] private GameObject futureObjects;
    [SerializeField] private GameObject pastObjects;

    private void Start()
    {
        TimeTravel.OnTimeChange.AddListener(OnTimeTravel);
        EnablePast();
    }

    private void OnTimeTravel()
    {
        if (TimeTravel.isFuture) EnableFuture();
        else EnablePast();
    }

    private void EnableFuture()
    {
        futureObjects.SetActive(true);
        pastObjects.SetActive(false);
    }
    
    private void EnablePast()
    {
        pastObjects.SetActive(true);
        futureObjects.SetActive(false);
    }
}
