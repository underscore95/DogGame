using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages NPC logic
public class NPC : MonoBehaviour, I_Interactable
{
    [SerializeField] private float _interactDistance = 5.0f;
    [SerializeField] private UI_BUBBLE_DISPLAY _display;
    [SerializeField] private UI_BUBBLE_DISPLAY _thoughtBubble;

    [SerializeField] private GameObject _smokePuffPrefab;

    [Header("Finish Game")]
    [SerializeField] private bool _canFinishGameByInteracting = false;
    [SerializeField] private Image _outroImage;
    [SerializeField] private float _fadeDuration = 2.0f;
    [SerializeField] private float _waitDuration = 1.0f; // after fade duration
    [SerializeField] private string _menuScene = "Menu";

    [Header("Clothing Item")]
    [SerializeField] private bool _hasClothing = true; // can't trade for clothing if this is false
    [SerializeField] private NPCModelAttachment _clothing;
    [SerializeField] private GameObject _clothingPrefab;
    [SerializeField] private ClothingItemType _clothingItemType;
    [SerializeField] private Material _materialNoBurn;
    QUEST_REFERENCER QR;


    [SerializeField] bool requiresItem;
    [SerializeField] string RequiredItemName;
    PLAYER_FETCHING PF;

    [Header("Sounds")]
    [SerializeField] AudioClip enterRangeCantTrade;
    [SerializeField] AudioClip enterRangeCanTrade;
    [SerializeField] AudioClip successfulTrade;
    [SerializeField] AudioClip succesfulTradeFX;
    [SerializeField] AudioClip failedTrade;
    AudioSource AS;
    CLOTHTRACKER_UI CLOTHUI;
    public int MoneyGiveAmount;
    [SerializeField] UnityEvent EventOnInteract;

    private PlayerClothing _playerClothing;
    private NPCModel _npcModel;
    public bool _hasTraded = false;
    public bool _activated;
    public int id;
    public int onCompleteID = 444;
    private float _secondsSinceGameFinish = -1;

    PLAYER_MOVEMENT pm;
    PLAYER_STATES st;

    private void Awake()
    {
       

        _npcModel = GetComponentInChildren<NPCModel>();
        CLOTHUI = GameObject.Find("ClothesCounterUI").GetComponent<CLOTHTRACKER_UI>();
        _activated = true;
        _playerClothing = FindAnyObjectByType<PlayerClothing>();
        PF = FindAnyObjectByType<PLAYER_FETCHING>();
        pm = PF.gameObject.GetComponent<PLAYER_MOVEMENT>();
        st = PF.gameObject.GetComponent<PLAYER_STATES>();
       QR = GetComponent<QUEST_REFERENCER>();
        AS = GetComponent<AudioSource>();

       // transform.localPosition = new Vector3(37 + id * 5, -1.5f, -33);

        if (_canFinishGameByInteracting)
        {
            Assert.IsNotNull(_outroImage, "must have an image to display for outro, in level blockout scene this is in the game finish canvas");
        }
    }

    private void Start()
    {
        if (_hasClothing)
        {
            Assert.IsNotNull(_clothing, $"Cannot have null clothing on {_npcModel.name}");
            Assert.IsTrue(_npcModel.HasAttachment(_clothing), $"You must add {_clothing.name} as an attachment to the NPCModel script on {_npcModel.name}");
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
            _secondsSinceGameFinish += Time.unscaledDeltaTime;
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
           // if (!_playerClothing.HasClothingItem(type)) return;
        }

        _secondsSinceGameFinish = 0;
    }

    public void EndGame()
    {
        _secondsSinceGameFinish = 0;

    }

    public void SetUpForEndTrade()
    {
        _canFinishGameByInteracting = true;
        _activated = true;
        requiresItem = false;
        _hasTraded = false;
    }

    private void HandleTradingForClothing()
    {
        if (!_activated)
        {
            return;
        }

        Instantiate(_smokePuffPrefab).transform.position = transform.position;
        _npcModel.RemoveAttachment(_clothing);
        CLOTHUI.CallStampPolaroid(_clothingItemType);
        StartCoroutine(Suncreamanimplayer());
        _hasTraded = true;
        _playerClothing.WearClothing(_clothingItemType, _clothingPrefab);

    }

    IEnumerator Suncreamanimplayer()
    {
        st.GState = PLAYER_STATES.GrndStates.Suncream;
        pm.cutscene = true;
        yield return new WaitForSeconds(3);
        pm.cutscene = false;
        st.GState = PLAYER_STATES.GrndStates.Idle;
        _npcModel.GetComponentInChildren<SkinnedMeshRenderer>().material = _materialNoBurn;


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
        if (EventOnInteract != null && !_canFinishGameByInteracting)
        { EventOnInteract.Invoke(); }

        if (_hasTraded) return;

        if (CanTrade())
        {
            if (requiresItem)
            {
                PF.DropObject();
                PF.heldFetch.canFetch = false;
                Destroy(PF.heldObj, 1.5f);
                PF.heldObj = null;
                PF.heldFetch = null;
            }
        }
        else
        { AS.PlayOneShot(failedTrade); _display.OnInteract(false); _thoughtBubble.OnInteract(false); _thoughtBubble.FX.MoveIn(Vector3.down * 0.5f, 12f);
            return; }
           

        _display.SetBubbleVisibility(false);
        _display.OnInteract(true);
        _thoughtBubble.OnInteract(true);
        _thoughtBubble.SetBubbleVisibility(false);
        AS.PlayOneShot(successfulTrade);

        AS.PlayOneShot(succesfulTradeFX);
        if (MoneyGiveAmount > 0)
        {
            QR.EN.AddMoney(MoneyGiveAmount);
            _hasTraded = true;
        }
        if (_hasClothing)
        {

            HandleTradingForClothing();
        }
        HandleQuesting();
    }

    public bool CanTrade()
    {
        if (!requiresItem) return true;
        if (PF.heldObj == null) return false;
        if (!PF.fetching) return false;
        if (requiresItem && PF.heldObj.name == RequiredItemName) { return true; }
        return false;
    }


    public void OnEnterInteractRange()
    {
        if (_hasTraded || !_activated) return;

        AS.PlayOneShot(CanTrade() ? enterRangeCanTrade : enterRangeCantTrade);
        _display.SetBubbleVisibility(true);
        _thoughtBubble.SetBubbleVisibility(true);
    }

    public void OnExitInteractRange()
    {
        _display.SetBubbleVisibility(false);
        _thoughtBubble.SetBubbleVisibility(false) ;

    }
}
