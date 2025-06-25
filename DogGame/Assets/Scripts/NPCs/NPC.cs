using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages NPC logic
public class NPC : MonoBehaviour, I_Interactable
{
    [SerializeField] private float _interactDistance = 5.0f;
    [SerializeField] private UI_BUBBLE_DISPLAY _display;

    [Header("Clothing Item")]
    [SerializeField] private bool _hasClothing = true; // can't trade for clothing if this is false
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
        QR = GetComponent<QUEST_REFERENCER>();

    }

    public void AttachQuest(int id)
    {
        onCompleteID = id;
    }

    private void Update()
    {

    }

    private void HandleTradingForClothing()
    {
        if (!_activated) { 
            return;
        }

        _playerClothing.WearClothing(_clothingItemType, _clothingObject);
        _clothingObject = null;

        _hasTraded = true;

        if (onCompleteID == 444) return;
        QR.QUESTS.Game_Quests[onCompleteID].DoComplete();
    }

    public void InteractableInRange()
    {
    }

    public void InteractableAction()
    {
        _display.SetBubbleVisibility(false);
        if (_hasTraded) return;
        if (_hasClothing)
        {
            HandleTradingForClothing();
        }
    }

    public void OnEnterInteractRange()
    {
        if (_hasTraded) return;
        _display.SetBubbleVisibility(true);
    }

    public void OnExitInteractRange()
    {
        _display.SetBubbleVisibility(false);
    }
}
