using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CLOTHTRACKER_UI : MonoBehaviour
{
    [SerializeField] UI_FX Hat;
    [SerializeField] UI_FX Glasses;
    [SerializeField] UI_FX Lei;
    [SerializeField] UI_FX Shirt;
    [SerializeField] UI_FX BG;

    [SerializeField] Sprite HatStamped;
    [SerializeField] Sprite GlassesStamped;
    [SerializeField] Sprite LeiStamped;
    [SerializeField] Sprite ShirtStamped;

    public bool hatObtained;
    public bool glassesObtained;
    public bool leiObtained;
    public bool shirtObtained;
    public bool allItemsObtained;

    public float fadeinSpd;
    public float fadeoutSpd;
    public float moveInSpd;
    public float moveInAmount;
    public float startDelay;
    public float stampDelay;
    public float appearTime;
    int MoneyAmount;
    int ActualMoney;
    bool open;
    AudioSource AS;
    [SerializeField] AudioClip stampSound;
    [SerializeField] AudioClip showSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
        Hat.BeginFadeOut(0f, 88f);
        Glasses.BeginFadeOut(0f, 66f);
        Lei.BeginFadeOut(0f, 88f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
   

    public void CallStampPolaroid(ClothingItemType cType)
    {
        if (!open)
        {
            open = true;
            StartCoroutine(DelayedOpen(cType));
        }
    }

    IEnumerator DelayedOpen(ClothingItemType cType)
    {
        yield return new WaitForSeconds(startDelay);
        OpenAnim();
        StartCoroutine(DelayStampPolaroid(cType));
    }

    IEnumerator DelayStampPolaroid(ClothingItemType cType)
    {
        yield return new WaitForSeconds(stampDelay);
        switch (cType)
        {
            case ClothingItemType.Hat:
                hatObtained = true; StampPolaroid(Hat, HatStamped); 
                break;
                case ClothingItemType.Glasses: glassesObtained = true; StampPolaroid(Glasses, GlassesStamped);  break;
                case ClothingItemType.Lei: leiObtained = true; StampPolaroid(Lei, LeiStamped);  break;
            case ClothingItemType.Shirt: shirtObtained = true; StampPolaroid(Shirt, ShirtStamped);  break;


        }
    }



    void OpenAnim()
    {
        Hat.EndFadeOut(fadeoutSpd);
        Glasses.EndFadeOut(fadeoutSpd);
        Lei.EndFadeOut(fadeoutSpd);
        Shirt.EndFadeOut(fadeoutSpd);
        BG.EndFadeOut(fadeoutSpd);

        Hat.MoveIn(Vector3.up * moveInAmount, moveInSpd);
        Glasses.MoveIn(Vector3.up * moveInAmount, moveInSpd * 0.85f);
        Lei.MoveIn(Vector3.up * moveInAmount, moveInSpd * 0.65f);
        Shirt.MoveIn(Vector3.up * moveInAmount, moveInSpd * 0.5f);
       



        BG.MoveIn(Vector3.up * moveInAmount, moveInSpd * 0.65f);
    }

    void StampPolaroid(UI_FX Polaroid, Sprite NewSprite)
    {
        Polaroid.img.sprite = NewSprite;
        Polaroid.ColorPulse(7f, Color.grey, 1f);
        Polaroid.ScalePulse(Vector3.one * 1.75f, Vector3.zero, 4f, 0f, true);
        AS.PlayOneShot(stampSound);
        StartCoroutine(DelayedClose());

        if (hatObtained && glassesObtained && shirtObtained && leiObtained)
        {
            allItemsObtained = true;
        }
    }

    void CloseAnim()
    {
        
        BG.BeginFadeOut(0f, fadeoutSpd);
        Hat.BeginFadeOut(0f, fadeoutSpd);
        Glasses.BeginFadeOut(0f, fadeoutSpd);
        Lei.BeginFadeOut(0f, fadeoutSpd);
        Shirt.BeginFadeOut(0f, fadeoutSpd);

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
