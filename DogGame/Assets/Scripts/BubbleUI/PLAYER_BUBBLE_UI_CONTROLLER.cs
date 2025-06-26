using NUnit.Framework;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PLAYER_BUBBLE_UI_CONTROLLER : MonoBehaviour
{
    [SerializeField] UI_BUBBLE_DISPLAY playerBubbleDisplay;
    [SerializeField] PLAYER_AUDIO PA;
    PLAYER_ANIMATION PANIM;
    PLAYER_INPUTS PI;
    UI_FX FX;
    private bool _interactInput = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PANIM = GetComponentInParent<PLAYER_ANIMATION>();
       
        PI = GetComponentInParent<PLAYER_INPUTS>();
        playerBubbleDisplay.SetSpriteDataIndex(0);
        Invoke(nameof(ShowBubble), 0.5f);
    }

    private void Update()
    {
        if (PI.IA_Interact_Bark.WasPressedThisFrame() && !_interactInput)
        {
            StartCoroutine(InputBuffer());
            PA.PlayBark();
            PANIM.PlayBarkAnim();

        }

        if (PI.IA_Move.WasPressedThisFrame())
        {
            if (!playerBubbleDisplay.hidden)
            {
                if (Time.timeScale != 0)
                {
                    playerBubbleDisplay.FX.ScalePulse(new Vector3(2f, 2f, 2f), Vector3.zero, 8f, 0f, true);
                }
            }
            Invoke(nameof(HideBubble), 1f);
        }
    }

    private IEnumerator InputBuffer()
    {
        _interactInput = true;
        yield return new WaitForSeconds(0.3f);
        _interactInput = false;
    }

    void ShowBubble()
    {
        playerBubbleDisplay.SetBubbleVisibility(true);
    }

    void HideBubble()
    {
        playerBubbleDisplay.SetBubbleVisibility(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<I_Interactable>() == null) return;

        other.GetComponent<I_Interactable>().OnEnterInteractRange();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<I_Interactable>() == null) return;

        other.GetComponent<I_Interactable>().OnExitInteractRange();
    }

    private void OnTriggerStay(Collider other) // Only works for objects with rigid bodies
    {
        if (other.GetComponent<I_Interactable>() == null) return;

        other.GetComponent<I_Interactable>().InteractableInRange();
        // trigger in range effect on interactable

        if (_interactInput)
        {
            _interactInput = false;
            other.GetComponent<I_Interactable>().InteractableAction(); // trigger interact effect on interactable
            playerBubbleDisplay.OnInteract();
        }
    }
}
