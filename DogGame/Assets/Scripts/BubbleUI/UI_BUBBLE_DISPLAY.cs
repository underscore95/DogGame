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

    private void Start()
    {
        SetBubbleVisibility(false);
    }

    public void SetBubbleVisibility(bool isVisible)
    {
        if (isVisible) 
        {
            StartUIAnimation();
        }
        else
        {
            image.color = alpha0;
            StopUIAnimation();
        }

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
        isPlaying = true;
        animFPS = spritesData[spritesDataIndex].animFPS;
        sprites = spritesData[spritesDataIndex].sprites;
        coroutineAnimation = StartCoroutine(PlayUIAnimation());
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
