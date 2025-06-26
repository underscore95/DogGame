using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BUTTON : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool clicked;
    public bool hovered;
    public bool selected;

    UI_FX FX;
    Button BTON;

    void Start()
    {
        BTON = GetComponent<Button>();
        FX= GetComponent<UI_FX>();
    }

    void Update()
    {
      //  if (hovered)
     //   { FX.ScalePulse(Vector3.one * 1.5f, new Vector3(0, 0, 60), 10f, 10f, false); }
      //  else
     //   {
     //       FX.ScalePulse(Vector3.one, Vector3.zero, 10f, 10f, false );
      //  }
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        Debug.Log("selected");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;   
        Debug.Log("deselected");
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        hovered = true;
        FX.OverrideScaleRotation(Vector3.one * 1.5f, new Vector3(0, 0, 50));
        Debug.Log("pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FX.StopOverrideScaleRotation();
        hovered = false;
        Debug.Log("pointer exit");
    }
}