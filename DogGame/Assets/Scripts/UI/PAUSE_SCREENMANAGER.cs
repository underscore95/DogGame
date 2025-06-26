using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PAUSE_SCREENMANAGER : MonoBehaviour
{
    GameObject player;
    PLAYER_INPUTS PI;
    public bool isPaused;
    [SerializeField] GameObject enable;

    [SerializeField] GameObject BG;
    UI_FX BGFX;
    public Button[] Buttons;
    UI_BUTTON[] UI_Buttons;
    bool fadingOut;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        BGFX = BG. GetComponent<UI_FX>();
        player = GameObject.Find("Player");
        PI = player.GetComponent<PLAYER_INPUTS>();

        UI_Buttons = new UI_BUTTON[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++) 
        {
            UI_Buttons[i] = Buttons[i].GetComponent<UI_BUTTON>();
        }
        //enable.SetActive(false);
      StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        enable.SetActive(false);
        UnPauseGame();
        AllObjFadeout();

    }



    // Update is called once per frame
    void Update()
    {
        if (PI.IA_Pause.WasPressedThisFrame())
        {
          if (!isPaused) { PauseGame(); }   else { UnPauseGame(); }
        }
        CheckForSelect();
    }

    void CheckForSelect()
    {
        bool one = false;
        for (int i = 0; i < Buttons.Length; i++) 
        {
            if (UI_Buttons[i].selected) { one = true; }
        }
        if (!one) { Buttons[0].Select(); }
    }

    public void PauseGame()
    {
        if (fadingOut) { return; }
        Cursor.visible = true;
        isPaused =  true;
        Time.timeScale = 0f;
        enable.SetActive(true);
        Buttons[0].Select();
        AllObjFadein();
        
    }

    public void AllObjFadeout()
    {
        for (int i = 0; i < UI_Buttons.Length; i++)
        {
            UI_Buttons[i].FX.BeginFadeOut(0f, 7f);
            Buttons[i].interactable = false;
        }
        BGFX.BeginFadeOut(0f, 7f);
    }

    public void AllObjFadein()
    {
        for (int i = 0; i < UI_Buttons.Length; i++)
        {
            UI_Buttons[i].FX.EndFadeOut(20f);
            Buttons[i].interactable = true;

        }
        BGFX.EndFadeOut(20f);
    }

    public void UnPauseGame()
    {
        fadingOut = true;
        AllObjFadeout();
        StartCoroutine(UnPauseDelay());
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    IEnumerator UnPauseDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
       
        enable.SetActive(false);
        fadingOut = false;

    }
}
