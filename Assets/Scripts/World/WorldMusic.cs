using UnityEngine;

public class WorldMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeTravel.OnTimeChange += TimeMusic;
    }

    void TimeMusic(bool isFuture)
    {
        if (isFuture)
        {
            MusicManager.instance.ChangeMusic(1);
        }
        else
        {
            MusicManager.instance.ChangeMusic(2);
        }
    }
}
