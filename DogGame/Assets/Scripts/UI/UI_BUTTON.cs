using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.Collections;

public class UI_BUTTON : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool clicked;
    public bool hovered;
    public bool selected;
    public bool transitional;
    public float selectRotate;
    public float selectScale;

    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip clickSound;


    public UnityEvent[] EventsOnClick;


    bool invokeButton;
    AudioSource aSrc;
    public UI_FX FX;
    UnityEngine.UI.Button BTON;
    bool invokeDelayb;
    public bool dontInteract;
    void Start()
    {
        aSrc = GetComponent<AudioSource>();
        BTON = GetComponent<UnityEngine.UI.Button>();
        BTON.onClick.AddListener(OnClick);
        FX= GetComponent<UI_FX>();
       
    }


    void Update()
    {
     if (dontInteract)
        { BTON.interactable = false; }
    }

    public void OnClick()
    {
        float time = transitional ? 2f: 0f;
        aSrc.PlayOneShot(clickSound);
        invokeDelayb = true;
        //EventOnClick.Invoke();
        Debug.Log(EventsOnClick.Length);
        for (int i = 0; i < EventsOnClick.Length; i++)
        {
            EventsOnClick[i].Invoke();
        }
    }

    

    

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        hovered = true;
        FX.OverrideScaleRotation(Vector3.one * selectScale, new Vector3(0, 0, selectRotate));
       
        aSrc.PlayOneShot(selectSound);
    }
    private void OnEnable()
    {
        if (FX != null)
        {
            FX.MoveIn(Vector3.down * 100f, 8f);
            FX.ColorPulse(6f, Color.white, 0f);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;   
        FX.StopOverrideScaleRotation();
        hovered = false;
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        hovered = true;
        BTON.Select();
        FX.OverrideScaleRotation(Vector3.one * selectScale, new Vector3(0, 0, selectRotate));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FX.StopOverrideScaleRotation();
        hovered = false;
    }
}