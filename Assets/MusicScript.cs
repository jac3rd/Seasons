using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void PlayMusic() {
        if(audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void StopMusic() {
        audioSource.Stop();
    }

    public void AdjustVolume(float volume) {
        if(volume < 0) volume = 0;
        else if(volume > 1) volume = 1;
        audioSource.volume = volume;
    }
}
