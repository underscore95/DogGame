using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.VFX;

public class PlayerClothing : MonoBehaviour
{
    [System.Serializable]
    public struct ClothingTransform
    {
        public ClothingItemType Type;
        public Transform Transform;
    }

    [SerializeField] private float _clothingEquipDuration = 0.5f;
    [SerializeField] private GameObject _poofEffectPrefab;
    [SerializeField] private List<ClothingTransform> _clothingTransforms = new();
    private readonly Dictionary<ClothingItemType, GameObject> _wornClothing = new();

    [SerializeField] GameObject shirt;

    private void Start()
    {
        shirt.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// Make the player wear a piece of clothing, replacing the existing piece of clothing the player is wearing, if any
    /// </summary>
    /// <param name="type">Clothing type</param>
    /// <param name="clothingPrefab">Clothing prefab</param>
    public void WearClothing(ClothingItemType type, GameObject clothingPrefab)
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

        if (type != ClothingItemType.Shirt)
        {
            GameObject clothingObject = Instantiate(clothingPrefab, parentTransform);
            _wornClothing[type] = clothingObject;
        }
        else 
        { 
            shirt.transform.localScale = Vector3.one;
        }



        // Linearly interpolate to be attached to the player:
        // 1. store original local transform
        // 2. reparent to player but modify local transform so that the world transform doesn't change
        // 3. interpolate the local transform back to the original
        //    InterpolatedTransform.LocalTransform destination = new(clothingObject.transform);
        //      clothingObject.transform.SetParent(parentTransform, true);
        //  InterpolatedTransform.StartInterpolation(clothingObject, destination, _clothingEquipDuration);

        StartCoroutine(PlayPoofEffect());
    }

    private IEnumerator PlayPoofEffect()
    {
        yield return new WaitForSeconds(_clothingEquipDuration);
        Instantiate(_poofEffectPrefab).transform.position = transform.position;
    }

    public bool HasClothingItem(ClothingItemType type)
    {
        return _wornClothing.ContainsKey(type);
    }
}
