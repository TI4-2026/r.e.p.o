using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour//singleton
{

    public AudioSource[] musics;
    public static MusicManager instance;
    private int currentMusic;
    private float fadeDuration = 1f;
    public AudioMixerGroup sfxMixer;
    public static AudioMixerGroup sfxMixerGroup;

    private void Awake()
    {
        sfxMixerGroup = sfxMixer;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < musics.Length; i++)
        {
            musics[i].Play();
            if (i != 0)
                musics[i].volume = 0;
        }
    }

    public void PlayMusic(int music)
    {
        if (currentMusic == music)
        {
            musics[music].Play();
            return;
        }
        for (int i = 0; i < musics.Length; i++)
        {
            if (i != music)
            {
                musics[i].volume = 0;
            }
            else
            {
                musics[i].Play();
                musics[i].volume = 1f;
                currentMusic = i;
            }
        }
    }

    public void ChangeMusic(int music)
    {
        if (currentMusic == music)
        {
            return;
        }

        AudioSource oldMusic = musics[currentMusic];
        AudioSource newMusic = musics[music];

        LeanTween.cancel(oldMusic.gameObject);
        LeanTween.cancel(newMusic.gameObject);

        LeanTween.value(oldMusic.gameObject, oldMusic.volume, 0f, fadeDuration)
            .setOnUpdate(value => oldMusic.volume = value);

        LeanTween.value(newMusic.gameObject, newMusic.volume, 1f, fadeDuration)
            .setOnUpdate(value => newMusic.volume = value);

        currentMusic = music;
    }

    public static void PlaySound(AudioClip clip, Vector2 position)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.outputAudioMixerGroup = sfxMixerGroup;
        aSource.Play();
        GameObject.Destroy(tempGO, clip.length);
    }
}