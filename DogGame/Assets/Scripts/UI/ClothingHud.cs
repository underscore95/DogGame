using UnityEngine;

public class ClothingHud : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Sprite[] _spritesObtained;
    [SerializeField] private ClothingItemType _type;

    private PlayerClothing _clothing;
    private ImageAnimation _imageAnimation;

    private void Awake()
    {
        _clothing = FindAnyObjectByType<PlayerClothing>();
        _imageAnimation = GetComponent<ImageAnimation>();
    }

    private void Update()
    {
        _imageAnimation.sprites = _clothing.HasClothingItem(_type) ? _spritesObtained : _sprites;
    }
}
