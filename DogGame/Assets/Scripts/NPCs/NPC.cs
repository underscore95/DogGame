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
    QUEST_REFERENCER QR;

    private PlayerClothing _playerClothing;
    public bool _hasTraded = false;
    public bool _activated;
    public int id;
    public int onCompleteID = 444;

    private void Awake()
    {
        _playerClothing = FindAnyObjectByType<PlayerClothing>();

        _tradeText.text = string.Format(_tradeText.text, _tradeInput.action.GetBindingDisplayString());
        QR = GetComponent<QUEST_REFERENCER>();
       
    }

    public void AttachQuest(int id)
    {
        onCompleteID = id;
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
        if (!_activated) { return; }
        if (!_tradeInput.action.WasPressedThisFrame()) return;

        _playerClothing.WearClothing(_clothingItemType, _clothingObject);
        _clothingObject = null;

        _hasTraded = true;
        _tradeText.enabled = false;

        if (onCompleteID == 444) return;
        QR.QUESTS.Game_Quests[onCompleteID].DoComplete();
    }
}
