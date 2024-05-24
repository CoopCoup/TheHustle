using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip EnemyShoot;
    public AudioClip Cash;
    public AudioSource loopingAudioSource;
    public AudioClip Music;
    public AudioClip PlayerDeath;
    public AudioClip PlayerShoot;
    public AudioClip EyeSpawn;
    public AudioClip Roulette;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        loopingAudioSource = gameObject.AddComponent<AudioSource>();

        loopingAudioSource.clip = Music;
        loopingAudioSource.loop = true;
    }

   
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "EnemyShoot":
                audioSource.PlayOneShot(EnemyShoot);
                break;

            case "Cash":
                audioSource.PlayOneShot(Cash);
                break;

            case "PlayerDeath":
                audioSource.PlayOneShot(PlayerDeath);
                break;

            case "PlayerShoot":
                audioSource.PlayOneShot(PlayerShoot);
                break;

            case "EyeSpawn":
                audioSource.PlayOneShot(EyeSpawn);
                break;

            case "Roulette":
                audioSource.PlayOneShot(Roulette);
                break;
        }
    }
    
    
    public void PlayLoopingSound()
    {
        if (!loopingAudioSource.isPlaying)
        {
            loopingAudioSource.Play();
        }
    }


    public void PauseLoopingSound()
    {
        if (loopingAudioSource.isPlaying)
        {
            loopingAudioSource.Pause();
        }
    }

    public void ResumeLoopingSound()
    {
        if (!loopingAudioSource.isPlaying)
        {
            loopingAudioSource.UnPause();
        }
    }

    public void StopLoopingSound()
    {
        if (loopingAudioSource.isPlaying)
        {
            loopingAudioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
