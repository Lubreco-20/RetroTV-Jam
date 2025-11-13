using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer master;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string mixerMusic = "MusicVolume";
    private const string mixerSfx = "SfxVolume";

    private static float musicValue;
    private static float sfxValue;

    private bool isMusicMuted;
    private bool isSfxMuted;

    void Awake()
    {
        musicSlider.minValue = 0.0001f;
        musicSlider.maxValue = 1f;
        sfxSlider.minValue = 0.0001f;
        sfxSlider.maxValue = 1f;

        musicValue = PlayerPrefs.GetFloat(mixerMusic, 0.5f);
        sfxValue = PlayerPrefs.GetFloat(mixerSfx, 0.5f);

        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    private void Start()
    {
        ApplySavedVolumes();
    }

    void ApplySavedVolumes()
    {
        float musicDb = Mathf.Log10(musicValue) * 20;
        float sfxDb = Mathf.Log10(sfxValue) * 20;

        master.SetFloat(mixerMusic, musicDb);
        master.SetFloat(mixerSfx, sfxDb);

        PlayerPrefs.SetFloat(mixerMusic, musicDb);
        PlayerPrefs.SetFloat(mixerSfx, sfxDb);
    }

    void SetMusicVolume(float value)
    {
        musicValue = value;
        if (!isMusicMuted)
        {
            float volumeDb = Mathf.Log10(value) * 20;
            master.SetFloat(mixerMusic, volumeDb);
            PlayerPrefs.SetFloat(mixerMusic, value);
        }
    }

    void SetSfxVolume(float value)
    {
        sfxValue = value;
        if (!isSfxMuted)
        {
            float volumeDb = Mathf.Log10(value) * 20;
            master.SetFloat(mixerSfx, volumeDb);
            PlayerPrefs.SetFloat(mixerSfx, value);
        }
    }

    public void MuteMusic(bool isMuted)
    {
        isMusicMuted = isMuted;
        master.SetFloat(mixerMusic, isMuted ? -80f : Mathf.Log10(musicValue) * 20);
    }

    public void MuteSfx(bool isMuted)
    {
        isSfxMuted = isMuted;
        master.SetFloat(mixerSfx, isMuted ? -80f : Mathf.Log10(sfxValue) * 20);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(mixerMusic, musicValue);
        PlayerPrefs.SetFloat(mixerSfx, sfxValue);
        PlayerPrefs.Save();
    }
}
