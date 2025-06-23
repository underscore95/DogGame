using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages NPC logic
public class NPC : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 5.0f;
    [SerializeField] private InputActionReference _tradeInput;
    [SerializeField] private TextMeshProUGUI _tradeText;

    [Header("Clothing Item")]
    [SerializeField] private GameObject _clothingObject;
    [SerializeField] private ClothingItemType _clothingItemType;

    private PlayerClothing _playerClothing;
    private bool _hasTraded = false;

    private void Awake()
    {
        _playerClothing = FindAnyObjectByType<PlayerClothing>();

        _tradeText.text = string.Format(_tradeText.text, _tradeInput.action.GetBindingDisplayString());
    }

    private void Update()
    {
        if (_hasTraded) return;

        bool playerIsNearby = Vector3.SqrMagnitude(_playerClothing.transform.position - transform.position) < _interactDistance * _interactDistance;
        _tradeText.enabled = playerIsNearby;

        if (!playerIsNearby) return;

        HandleTrading();
    }

    private void HandleTrading()
    {
        if (!_tradeInput.action.WasPressedThisFrame()) return;

        _playerClothing.WearClothing(_clothingItemType, _clothingObject);
        _clothingObject = null;

        _hasTraded = true;
        _tradeText.enabled = false;
    }
}
