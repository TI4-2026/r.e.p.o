using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundConfig : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("UI")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        masterSlider.value = PlayerPrefs.GetFloat("Master", 0.7f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 0.6f);

        ApplyToMixer("MasterVolume", masterSlider.value);
        ApplyToMixer("MusicVolume", musicSlider.value);
        ApplyToMixer("SFXVolume", sfxSlider.value);
    }

    public void SetMasterVolume(float value)
    {
        ApplyToMixer("MasterVolume", value);
        PlayerPrefs.SetFloat("Master", value);
    }

    public void SetMusicVolume(float value)
    {
        ApplyToMixer("MusicVolume", value);
        PlayerPrefs.SetFloat("Music", value);
    }

    public void SetSFXVolume(float value)
    {
        ApplyToMixer("SFXVolume", value);
        PlayerPrefs.SetFloat("SFX", value);
    }

    private void ApplyToMixer(string parameter, float value)
    {
        float dB = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        mixer.SetFloat(parameter, dB);
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}