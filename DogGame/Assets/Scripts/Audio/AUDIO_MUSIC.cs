using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AUDIO_MUSIC : MonoBehaviour
{
    public static AUDIO_MUSIC Instance;

    private AudioSource levelAudioSource;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip pausedMusic;
    [SerializeField] AudioClip endScreenMusic;
    public float startVolume;
    private bool ended = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        levelAudioSource = gameObject.GetComponent<AudioSource>();        
        levelAudioSource.clip = levelMusic;
        levelAudioSource.Play();
        levelAudioSource.volume = startVolume;  
        
        
    }

    void Update()
    {
        
                // if paused stop
                if (Time.timeScale == 0 && ended == false)
                {
                    if (levelAudioSource.clip != pausedMusic)
                    {
                //  float playTime = levelAudioSource.time;
                // levelAudioSource.Stop();
                // levelAudioSource.clip = pausedMusic;
                // levelAudioSource.Play();
                // levelAudioSource.time = playTime;
                EndMusic();
            }
                }
                else
                {
                    if (levelAudioSource.clip != levelMusic && ended == false)
                    {

                        float playTime = levelAudioSource.time;
                        levelAudioSource.Stop();
                        levelAudioSource.clip = levelMusic;
                        levelAudioSource.Play();
                        levelAudioSource.time = playTime;
                    }
                }


     }

    public void EndMusic()
    {
        ended = true;
          
        levelAudioSource.clip = endScreenMusic;
        levelAudioSource.Play();                      
    }
 }


 
