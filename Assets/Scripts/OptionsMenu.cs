using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;

    [SerializeField]
    private AudioMixer _SFXMixer;

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        _SFXMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIdx)
    {
        QualitySettings.SetQualityLevel(qualityIdx);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}