using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] AudioSource source;
    public AudioClip[] clips;
    AudioMixer mixer;

    void Start()
    {
        if(source == null) source = GetComponent<AudioSource>();

        //GameState.OnStopSound += StopSoundEffect;
    }

    public void PlaySoundEffect(string nameClip, bool isOneShot = true)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == nameClip)
            {
                if (isOneShot)
                {
                    source.PlayOneShot(clip);
                }
                else
                {
                    source.clip = clip;
                    source.Play();
                }
                return;
            }
        }

        Debug.Log("No hay ningún sonido llamado: " + nameClip);
    }

    public void CanLoop(bool canLoop) => source.loop = canLoop;

    public void StopSoundEffect() => source.Stop();

    public void ChangeAudioMixerVolume(string nameVolume, float volume)
    {
        volume = Mathf.Clamp(volume, -80, 20);
        mixer = source.outputAudioMixerGroup.audioMixer;
        mixer.SetFloat(nameVolume, volume);
    }

    public void GetRandomSoundEffect(int minIndex, int maxIndex, bool isOneShot = true)
    {
        string nameClip = clips[Random.Range(minIndex, maxIndex)].name;
        PlaySoundEffect(nameClip, isOneShot);
    }

    public void PlaySoundEffectFromAnimator(string nameClip) => PlaySoundEffect(nameClip);
}
