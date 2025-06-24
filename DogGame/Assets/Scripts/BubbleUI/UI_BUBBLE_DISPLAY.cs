using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_BUBBLE_DISPLAY : MonoBehaviour
{
    [SerializeField] GameObject displayObject;

    [Header("UI Element")]
    [SerializeField] Image image;
    [SerializeField] SO_UI_BUBBLE_SPRITES[] spritesData;
    private Sprite[] sprites;
    //[SerializeField] int animFPS;

    int dataIndex;
    int spriteIndex;
    Coroutine coroutineAnimation;
    bool isPlaying;

    private void Start()
    {
        SetBubbleSprites(0);
        SetBubbleVisability(true);
    }

    public void SetBubbleVisability(bool isVisable)
    {
        displayObject.SetActive(isVisable);
        if (isVisable) 
        { 
            StartUIAnimation(); 
        } else
        {
            StopUIAnimation();
        }
    }

    /// <summary>
    /// Sets the displayed sprites from spritesData based on the index, spritesData is defined in the inspector.
    /// </summary>
    /// <param name="spritesDataIndex">Index of the sprite data to be loaded</param>
    public void SetBubbleSprites(int spritesDataIndex)
    {
        if (spritesDataIndex < spritesData.Length) // validaton
        {
            dataIndex = spritesDataIndex;
            sprites = spritesData[spritesDataIndex].sprites;
        }
        else Debug.LogWarning("spriteDataIndex provided was out of bounds of the array, sprites were not updated");
    }

    void StartUIAnimation()
    {
        isPlaying = true;
        coroutineAnimation = StartCoroutine(PlayUIAnimation());
    }

    private void StopUIAnimation()
    {
        isPlaying = false;
        StopCoroutine(coroutineAnimation);
    }

    IEnumerator PlayUIAnimation()
    {
        float delay = 1f / Mathf.Clamp(spritesData[dataIndex].animFPS, 1, 60);
        yield return new WaitForSeconds(delay);
        if (spriteIndex >=  sprites.Length) { spriteIndex = 0; }
        image.sprite = sprites[spriteIndex];
        spriteIndex++;
        if (isPlaying == true) 
        {
            coroutineAnimation = StartCoroutine(PlayUIAnimation()); 
        }
    }

}
