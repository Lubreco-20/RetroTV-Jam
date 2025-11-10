using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer master;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    string mixerMusic = "MusicVolume";
    string mixerSfx = "SfxVolume";

    static float musicValue;
    static float sfxValue;

    bool isMusicMuted;
    bool isSfxMuted;

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        musicSlider.minValue = 0.0001f;
        musicSlider.maxValue = 1;
        musicValue = PlayerPrefs.GetFloat(mixerMusic, 1f);

        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        sfxSlider.minValue = 0.0001f;
        sfxSlider.maxValue = 1;
        sfxValue = PlayerPrefs.GetFloat(mixerSfx, 1f);
    }

    private void Start()
    {
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
    }

    void SetMusicVolume(float value)
    {
        musicValue = Mathf.Log10(value) * 20;
        if (!isMusicMuted)
        {
            PlayerPrefs.SetFloat(mixerMusic, musicValue);
            master.SetFloat(mixerMusic, musicValue);
        }
    }
    
    void SetSfxVolume(float value)
    {
        sfxValue = Mathf.Log10(value) * 20;
        if (!isSfxMuted)
        {
            PlayerPrefs.GetFloat(mixerSfx, sfxValue);
            master.SetFloat(mixerSfx, sfxValue);
        }
    }

    public void MuteMusic(bool isMuted)
    {
        isMusicMuted = isMuted;
        master.SetFloat(mixerMusic, isMusicMuted ? -80f : musicValue);
    }

    public void MuteSfx(bool isMuted)
    {
        isSfxMuted = isMuted;
        master.SetFloat(mixerSfx, isSfxMuted ? -80f : sfxValue);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(mixerMusic, musicSlider.value);
        PlayerPrefs.SetFloat(mixerSfx, sfxSlider.value);
    }
}
