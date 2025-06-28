using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

public class COINCOUNTER_UI : MonoBehaviour
{
    [SerializeField] UI_FX BG;
    [SerializeField] UI_FX Image;
    [SerializeField] UI_FX Text;
    public float fadeinSpd;
    public float fadeoutSpd;
    public float moveInSpd;
    public float moveInAmount;
    public float startDelay;
    public float addMoneyDelay;
    public float appearTime;
    float MoneyAmount;
    float ActualMoney;
    bool open;
    AudioSource AS;
    [SerializeField] AudioClip pickupSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
        Image.Rotating(Vector3.up, 100f);
        BG.BeginFadeOut(0f, 88f);
        Image.BeginFadeOut(0f, 66f);
        Text.BeginFadeOut(0f, 88f);
        //Text.text.text = ("£" + (MoneyAmount / 10).ToString());
        Text.text.text = ("£0.00");

    }

    // Update is called once per frame
    void Update()
    {
    }
   

    public void MoneyAdded(float CurrentMoney)
    {
        ActualMoney = CurrentMoney;
        if (!open)
        {
            open = true;
            StartCoroutine(DelayedOpen());
        }
    }

    IEnumerator DelayedOpen()
    {
        yield return new WaitForSeconds(startDelay);
        OpenAnim();
        StartCoroutine(DelayAddMoney(addMoneyDelay, 1));
    }

    void OpenAnim()
    {
        BG.EndFadeOut(fadeoutSpd);
        Image.EndFadeOut(fadeoutSpd);
        Text.EndFadeOut(fadeoutSpd);
        BG.MoveIn(Vector3.up * moveInAmount, moveInSpd);
        Image.MoveIn(Vector3.up * moveInAmount, moveInSpd);
        Text.MoveIn(Vector3.up * moveInAmount, moveInSpd);
    }

    void CloseAnim()
    {
        BG.BeginFadeOut(0f, fadeoutSpd);
        Image.BeginFadeOut(0f, fadeoutSpd);
        Text.BeginFadeOut(0f, fadeoutSpd);
    }

    IEnumerator DelayAddMoney(float time, float depth)
    {
        yield return new WaitForSeconds(time);
        MoneyAmount++;
        float pitch = Mathf.Pow(2, depth / 12);
        Debug.Log(pitch);
        Debug.Log(depth);
        AS.pitch = pitch;
        AS.PlayOneShot(pickupSound);
        string money = (MoneyAmount / 10).ToString();
        if (money.Length < 4 && money.Length > 2) 
        {
            money = (MoneyAmount / 10).ToString() + "0";
        }
        if (money.Length< 2)
        {
            money = (MoneyAmount / 10).ToString() + ".00";
        }
        Text.text.text = ("£" + money);
        Text.ColorPulse(4f, Color.green, 1f);
        Image.ColorPulse(4f, Color.green, 1f);
       //  Text.ScalePulse(Vector3.one * 1.5f, Vector3.zero, 10f, 0f, true);
        Text.MoveIn(Vector3.up * 25f, 8f);
        if (ActualMoney == MoneyAmount)
        {
            StartCoroutine(DelayedClose());
        }
        else
        {
            StartCoroutine(DelayAddMoney(time * 0.75f, depth + 1));
        }
    }

    IEnumerator DelayedClose()
    {
        yield return new WaitForSeconds(appearTime);
        CloseAnim();
        open = false;
    }

    public void Show()
    {
        OpenAnim();
    }

    public void Hide()
    {
        CloseAnim();
    }
}
