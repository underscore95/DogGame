using UnityEngine;

public class AUDIO_MUSIC : MonoBehaviour
{
    private AudioSource levelAudioSource;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip pausedMusic;

    void Start()
    {
        levelAudioSource = GetComponent<AudioSource>();
        levelAudioSource.clip = levelMusic;
        levelAudioSource.Play();
    }

    void Update()
    {
        
                // if paused stop
                if (Time.timeScale == 0)
                {
                    if (levelAudioSource.clip != pausedMusic)
                    {
                        float playTime = levelAudioSource.time;
                        levelAudioSource.Stop();
                        levelAudioSource.clip = pausedMusic;
                        levelAudioSource.Play();
                        levelAudioSource.time = playTime;
                    }
                }
                else
                {
                    if (levelAudioSource.clip != levelMusic)
                    {

                        float playTime = levelAudioSource.time;
                        levelAudioSource.Stop();
                        levelAudioSource.clip = levelMusic;
                        levelAudioSource.Play();
                        levelAudioSource.time = playTime;
                    }
                }

            }
        }
 
