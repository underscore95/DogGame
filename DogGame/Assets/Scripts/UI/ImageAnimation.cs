using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// modified: https://gist.github.com/almirage/e9e4f447190371ee6ce9
public class ImageAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public bool loop = true;
    public bool destroyOnEnd = false;
    public float fps = 5;

    private int index = 0;
    private Image image;
    private int frame = 0;
    private float _secondsSinceLastFrame = 0;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        _secondsSinceLastFrame += Time.deltaTime;
        if (_secondsSinceLastFrame < 1.0f / fps) return;
        _secondsSinceLastFrame = 0;

        if (!loop && index == sprites.Length) return;
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
