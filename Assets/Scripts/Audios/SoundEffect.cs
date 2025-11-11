using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] AudioSource source;
    public AudioClip[] clips;
    AudioMixer mixer;

    private float ogPitch;

    void Start()
    {
        if(source == null) source = GetComponent<AudioSource>();

        ogPitch = source.pitch;

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

    public void GetRandomSoundEffect()
    {
        string nameClip = clips[Random.Range(0, clips.Length)].name;
        PlaySoundEffect(nameClip, true);
    }

    public void GetRandomSoundEffect(int minInclude, int maxEsclude)
    {
        string nameClip = clips[Random.Range(minInclude, maxEsclude)].name;
        PlaySoundEffect(nameClip, true);
    }

    public void SetRandomPitch()
    {
        float rndPitch = Random.Range(0.75f, 1.5f);
        float newPitch = rndPitch * ogPitch;
        newPitch = Mathf.Clamp(newPitch, -3f, 3f);
        source.pitch = newPitch;
    }

    public void SetOGPitch()
        => source.pitch = ogPitch;

    public void SetPitch(int i)
        => source.pitch = i;

    public void PlaySoundEffectFromAnimator(string nameClip) => PlaySoundEffect(nameClip);
}
