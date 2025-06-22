using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerClothing : MonoBehaviour
{
    [System.Serializable]
    public struct ClothingTransform
    {
        public ClothingItemType Type;
        public Transform Transform;
    }

    [SerializeField] private List<ClothingTransform> _clothingTransforms = new();
    private readonly Dictionary<ClothingItemType, GameObject> _wornClothing = new();

    /// <summary>
    /// Make the player wear a piece of clothing, replacing the existing piece of clothing the player is wearing, if any
    /// </summary>
    /// <param name="type">Clothing type</param>
    /// <param name="prefab">Clothing prefab</param>
    public void WearClothing(ClothingItemType type, GameObject prefab)
    {
        // Destroy existing clothing
        if (_wornClothing.TryGetValue(type, out GameObject worn))
        {
            Destroy(worn);
            Debug.LogWarning($"Player equipped {type} clothing multiple times, is that intended?");
        }

        // Get transform
        Transform parentTransform = null;
        foreach (ClothingTransform clothingTransform in _clothingTransforms)
        {
            if (clothingTransform.Type == type)
            {
                parentTransform = clothingTransform.Transform;
                break;
            }
        }
        Assert.IsNotNull(parentTransform, $"No clothing transform configured for {type}");


        _wornClothing[type] = Instantiate(prefab, parentTransform);
    }
}
