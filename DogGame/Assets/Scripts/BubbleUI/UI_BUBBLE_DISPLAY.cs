using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_BUBBLE_DISPLAY : MonoBehaviour
{
    //[SerializeField] GameObject displayObject;

    [Header("UI Element")]
    [SerializeField] Image image;
    [SerializeField] SO_UI_BUBBLE_SPRITES[] spritesData;
    private Sprite[] sprites;
    int animFPS;
    int spriteIndex = 0;
    int spritesDataIndex = 0;
    Coroutine coroutineAnimation;
    bool isPlaying;
    Color alpha0 = new(0, 0, 0, 0);
    public UI_FX FX;
    public bool hidden;
    public bool dontUseAnim;
    public float appearSpd;
    public float disappearSpd;
    public float moveinSpd;
    public float pulseSpd;
    public float colorPulseSpd;

    private void Start()
    {
        FX = GetComponent<UI_FX>();
        SetBubbleVisibility(false);
    }

    public void SetBubbleVisibility(bool isVisible)
    {
        if (isVisible) 
        {
            StartUIAnimation();
            if (hidden) { OnAppear(); }
            hidden = false;
        }
        else
        {
            //image.color = alpha0;
            StopUIAnimation();
            if (!hidden) { OnDisappear(); }
            hidden = true;

        }

    }

    public void OnInteract(bool successful)
    {
        if (successful)
        {
            FX.ScalePulse(new Vector3(0.5f, 0.5f, 0.5f), Vector3.zero, 10f, 0f, true);
            FX.ColorPulse(colorPulseSpd, Color.green, 1f);
        }
        else
        {
            FX.ScalePulse(new Vector3(0.5f, 0.5f, 0.5f), Vector3.zero, pulseSpd, 0f, true);
            FX.ColorPulse(colorPulseSpd, Color.red, 1f);
        }
    }

    public void OnAppear()
    {
        FX.EndFadeOut(14f);
        FX.MoveIn(Vector3.down * 1f, appearSpd);

    }

    public void OnDisappear()
    {
        FX.BeginFadeOut(0, disappearSpd);
    }

    /// <summary>
    /// Sets the displayed sprites from spritesData based on the index, spritesData is defined in the inspector.
    /// </summary>
    /// <param name="i">Index of the sprite data to be loaded</param>
    public void SetSpriteDataIndex(int i)
    {
        if (i < spritesData.Length) // validaton
        {
            spritesDataIndex = i;
        }
        else Debug.LogWarning("spriteDataIndex provided was out of bounds of the array, sprites were not updated");
    }

    void StartUIAnimation()
    {
        if (!dontUseAnim)
        {
            isPlaying = true;
            animFPS = spritesData[spritesDataIndex].animFPS;
            sprites = spritesData[spritesDataIndex].sprites;
            coroutineAnimation = StartCoroutine(PlayUIAnimation());
        }
    }

    private void StopUIAnimation()
    {
        isPlaying = false;
        if (coroutineAnimation != null) 
            StopCoroutine(coroutineAnimation);
    }

    IEnumerator PlayUIAnimation()
    {
        float delay = 1f / Mathf.Clamp(animFPS, 1, 60);
        yield return new WaitForSeconds(delay);
        if (spriteIndex >=  sprites.Length) { spriteIndex = 0; }
        image.sprite = sprites[spriteIndex];
        spriteIndex++;
        if (isPlaying == true) 
        {
            image.color = Color.white;
            coroutineAnimation = StartCoroutine(PlayUIAnimation());
        }
    }
}
