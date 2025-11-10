using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;

    public string startSong;

    static MusicPlayer instance;
    public static MusicPlayer Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance.PlayMusic(startSong);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(startSong);
    }

    public void PlayMusic(string nameClip)
    {
        if(nameClip == "")
        {
            Debug.Log("No hay ninguna música puesta");
            return;
        }

        foreach (AudioClip clip in clips)
        {
            if (clip.name == nameClip)
            {
                // Check if the actual clip is playing
                if(source.clip != null) 
                    if (source.clip.name == nameClip && source.isPlaying) break;

                source.clip = clip;
                source.Play();
                break;
            }
        }

        if(source.clip.name != nameClip)
        {
            Debug.Log("No existe la música con el nombre: " + nameClip);
        }
    }
}
