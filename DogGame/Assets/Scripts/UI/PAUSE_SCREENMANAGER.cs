using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PAUSE_SCREENMANAGER : MonoBehaviour
{
    GameObject player;
    PLAYER_INPUTS PI;
    public bool isPaused;
    public bool enableFromStart;
    [SerializeField] GameObject enable;
    public bool StopTimeScale;
    [SerializeField] GameObject BG;
    UI_FX BGFX;
    public Button[] Buttons;
    UI_BUTTON[] UI_Buttons;
    bool fadingOut;
    COINCOUNTER_UI CCUI;
    GameObject CC;
    GameObject CLOTHTRACKOBJ;
    CLOTHTRACKER_UI CLOTHUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CC = GameObject.Find("CoinCounterUI");
        CLOTHTRACKOBJ = GameObject.Find("ClothesCounterUI");
        CLOTHUI = CLOTHTRACKOBJ.GetComponent<CLOTHTRACKER_UI>();
        CCUI = CC.GetComponent<COINCOUNTER_UI>();

        if (enableFromStart)
        { Cursor.visible = true; 
        }
        else { Cursor.visible = false; }
        BGFX = BG. GetComponent<UI_FX>();
          if (!enableFromStart)
          {
        player = GameObject.Find("Player");

        PI = player.GetComponent<PLAYER_INPUTS>();
            StartCoroutine(Delay());

        }

        UI_Buttons = new UI_BUTTON[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++) 
        {
            UI_Buttons[i] = Buttons[i].GetComponent<UI_BUTTON>();
        }
        //enable.SetActive(false);
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
        if (!enableFromStart)
        {
            if (PI.IA_Pause.WasPressedThisFrame())
            {
                if (!isPaused) { PauseGame(); } else { UnPauseGame(); }
            }
        }
        CheckForSelect();
    }

    void CheckForSelect()
    {
        bool one = false;
        for (int i = 0; i < UI_Buttons.Length; i++) 
        {
            if (UI_Buttons[i].selected) { one = true; }
        }
        if (!one) { Buttons[0].Select(); }
    }

    public void PauseGame()
    {
        if (fadingOut) { return; }
        Cursor.visible = true;
        isPaused = true;
        if (StopTimeScale)
        {
            Time.timeScale = 0f;
        }
        CCUI.Show();
        CLOTHUI.Show();
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
        CCUI.Hide();
        CLOTHUI.Hide();
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
