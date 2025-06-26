using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages NPC logic
public class NPC : MonoBehaviour, I_Interactable
{
    [SerializeField] private float _interactDistance = 5.0f;
    [SerializeField] private UI_BUBBLE_DISPLAY _display;
    [SerializeField] private GameObject _smokePuffPrefab;

    [Header("Finish Game")]
    [SerializeField] private bool _canFinishGameByInteracting = false;
    [SerializeField] private Image _outroImage;
    [SerializeField] private float _fadeDuration = 2.0f;
    [SerializeField] private float _waitDuration = 1.0f; // after fade duration
    [SerializeField] private string _menuScene = "Menu";

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
    private float _secondsSinceGameFinish = -1;

    private void Awake()
    {
        _playerClothing = FindAnyObjectByType<PlayerClothing>();
        QR = GetComponent<QUEST_REFERENCER>();

        transform.localPosition = new Vector3(37 + id * 5, -1.5f, -33);

        if (_canFinishGameByInteracting)
        {
            Assert.IsNotNull(_outroImage, "must have an image to display for outro, in level blockout scene this is in the game finish canvas");
        }
    }

    public void AttachQuest(int id)
    {
        onCompleteID = id;
    }

    private void Update()
    {
        if (_secondsSinceGameFinish >= 0)
        {
            _secondsSinceGameFinish += Time.deltaTime;
            Color c = _outroImage.color;
            c.a = Mathf.InverseLerp(0.0f, _fadeDuration, _secondsSinceGameFinish);
            _outroImage.color = c;
            if (_secondsSinceGameFinish >= _waitDuration + _fadeDuration)
            {
                SceneManager.LoadScene(_menuScene, LoadSceneMode.Single);
            }
        }
    }

    private void HandleFinishingGame()
    {
        if (!_canFinishGameByInteracting || _secondsSinceGameFinish >= 0) return;

        foreach (ClothingItemType type in Enum.GetValues(typeof(ClothingItemType)))
        {
            if (!_playerClothing.HasClothingItem(type)) return;
        }

        _secondsSinceGameFinish = 0;
    }

    private void HandleTradingForClothing()
    {
        if (!_activated)
        {
            return;
        }

        Instantiate(_smokePuffPrefab).transform.position = transform.position;
        _playerClothing.WearClothing(_clothingItemType, _clothingObject);
        _clothingObject = null;

        _hasTraded = true;
    }

    private void HandleQuesting()
    {
        if (!_activated)
        {
            return;
        }

        if (onCompleteID == 444) return;
        QR.QUESTS.Game_Quests[onCompleteID].DoComplete();
    }

    public void InteractableInRange()
    {
    }

    public void InteractableAction()
    {
        HandleFinishingGame();

        _display.SetBubbleVisibility(false);
        _display.OnInteract();
        if (_hasTraded) return;
        if (_hasClothing)
        {
            HandleTradingForClothing();
        }
        HandleQuesting();
    }

    public void OnEnterInteractRange()
    {
        if (_hasTraded || !_activated) return;
        _display.SetBubbleVisibility(true);
    }

    public void OnExitInteractRange()
    {
        _display.SetBubbleVisibility(false);
    }
}
