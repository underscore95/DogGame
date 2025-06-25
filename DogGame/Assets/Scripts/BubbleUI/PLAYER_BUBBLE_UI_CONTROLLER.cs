using NUnit.Framework;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PLAYER_BUBBLE_UI_CONTROLLER : MonoBehaviour
{
    [SerializeField] UI_BUBBLE_DISPLAY playerBubbleDisplay;
    [SerializeField] PLAYER_AUDIO PA;
    PLAYER_INPUTS PI;
    private bool _interactInput = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PI = GetComponentInParent<PLAYER_INPUTS>();
        playerBubbleDisplay.SetSpriteDataIndex(0);
        Invoke(nameof(ShowBubble), 0.5f);
    }

    private void Update()
    {
        if (PI.IA_Interact_Bark.WasPressedThisFrame())
        {
            StartCoroutine(InputBuffer());
            PA.PlayBark();
        }

        if (PI.IA_Move.WasPressedThisFrame())
        {
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
        }
    }
}
