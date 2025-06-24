using UnityEngine;

public class PLAYER_AUDIO : MonoBehaviour
{
    public AudioClip[] Footsteps;
    AudioSource AS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstep()
    {
        AudioClip clip = Footsteps[Random.Range(0, Footsteps.Length)];
        AS.PlayOneShot(clip);
    }
}
