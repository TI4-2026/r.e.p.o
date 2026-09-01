using UnityEngine;

public class PairsTimeTravel : MonoBehaviour
{
    public GameObject[] future;
    public GameObject[] past;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        TimeTravel.OnTimeChange += ChangeTime;
    }

    void ChangeTime(bool isFuture)
    {
        if (isFuture)
        {
            for (int i = 0; i < future.Length; i++)
            {
                past[i].SetActive(false);
                future[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < past.Length; i++)
            {
                future[i].SetActive(false);
                past[i].SetActive(true);
            }
        }
    }
}
