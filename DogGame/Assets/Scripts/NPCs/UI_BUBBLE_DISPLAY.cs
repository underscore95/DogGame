using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_BUBBLE_DISPLAY : MonoBehaviour
{
    [SerializeField] GameObject displayObject;

    [Header("UI Element")]
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int animFPS;

    int spriteIndex;
    Coroutine coroutineAnimation;
    bool isPlaying;

    private void Start()
    {
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
        float delay = 1f / Mathf.Clamp(animFPS, 1, 60);
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
