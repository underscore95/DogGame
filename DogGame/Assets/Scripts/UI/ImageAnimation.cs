using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// modified: https://gist.github.com/almirage/e9e4f447190371ee6ce9
public class ImageAnimation : MonoBehaviour
{
    /// <summary>
    /// in case you want to use the SO to make your life easier :), will override the sprites field unless this is null
    /// </summary>

    [Tooltip("in case you want to use the SO to make your life easier :), will override the sprites field unless this is null")]
    [SerializeField] SO_UI_BUBBLE_SPRITES spritesData;
    public Sprite[] sprites;
    public bool loop = true;
    public bool destroyOnEnd = false;
    public float fps = 5;
    public AUDIO_MUSIC Instance;

    private int index = 0;
    private Image image;
    private int frame = 0;
    private float _secondsSinceLastFrame = 0;

    void Awake()
    {
        image = GetComponent<Image>();
        if (spritesData != null ) { sprites = spritesData.sprites; }
        Instance.EndMusic();
    }

    void Update()
    {
        if (sprites == null || sprites.Length == 0) return;
        _secondsSinceLastFrame += Time.unscaledDeltaTime;
        if (_secondsSinceLastFrame < 1.0f / fps) return;
        _secondsSinceLastFrame = 0;

        if (!loop && index >= sprites.Length) return;
        frame++;
        image.sprite = sprites[index];
        frame = 0;
        index++;
        if (index >= sprites.Length)
        {
            if (loop) index = 0;
            if (destroyOnEnd) Destroy(gameObject);
        }
    }
}
