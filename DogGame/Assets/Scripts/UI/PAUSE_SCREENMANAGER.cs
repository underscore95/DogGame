using UnityEngine;

public class PAUSE_SCREENMANAGER : MonoBehaviour
{
    GameObject player;
    PLAYER_INPUTS PI;
    public bool isPaused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        PI = player.GetComponent<PLAYER_INPUTS>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PI.IA_Pause.WasPressedThisFrame())
        {
          if (!isPaused) { PauseGame(); }   else { UnPauseGame(); }
        }
    }

    public void PauseGame()
    {
        isPaused =  true;
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        isPaused=false;
    }
}
