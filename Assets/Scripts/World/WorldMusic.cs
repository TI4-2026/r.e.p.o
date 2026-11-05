using UnityEngine;

public class WorldMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeTravel.OnTimeChange.AddListener(TimeMusic);
    }

    void TimeMusic()
    {
        if (TimeTravel.isFuture)
        {
            MusicManager.instance.ChangeMusic(1);
        }
        else
        {
            MusicManager.instance.ChangeMusic(2);
        }
    }
}
