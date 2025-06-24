using System.Collections;
using UnityEngine;

public class PLAYER_AUDIO : MonoBehaviour
{
    public AudioClip[] Footsteps;
    public AudioClip[] Barks;
    public float barkRate;
    bool barking;
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

    public void PlayBark()
    {
        if (barking) { return; }
        AudioClip bark = Barks[Random.Range(0, Barks.Length)];
        AS.PlayOneShot(bark);
        Debug.Log("bark");
        StartCoroutine(BarkCooldown());
    }

    IEnumerator BarkCooldown()
    {
        barking = true;
        yield return new WaitForSeconds(barkRate);
        barking = false;
    }

    public void PlayFootstep()
    {
        AudioClip clip = Footsteps[Random.Range(0, Footsteps.Length)];
        AS.PlayOneShot(clip);
    }
}
