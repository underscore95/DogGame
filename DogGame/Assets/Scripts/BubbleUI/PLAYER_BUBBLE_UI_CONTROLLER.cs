using Unity.VisualScripting;
using UnityEngine;

public class PLAYER_BUBBLE_UI_CONTROLLER : MonoBehaviour
{
    [SerializeField] UI_BUBBLE_DISPLAY playerBubbleDisplay;
    PLAYER_INPUTS PI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PI = GetComponentInParent<PLAYER_INPUTS>();

        playerBubbleDisplay.SetSpriteDataIndex(0);
        Invoke(nameof(ShowBubble), 0.5f);
    }

    private void Update()
    {
        if (PI.IA_Move.WasPressedThisFrame())
        {
            Invoke(nameof(HideBubble), 1f);
        }
    }

    void ShowBubble()
    {
        playerBubbleDisplay.SetBubbleVisability(true);
    }

    void HideBubble()
    {
        playerBubbleDisplay.SetBubbleVisability(false);
    }


    private void OnTriggerStay(Collider other) // Only works for objects with rigid bodies
    {
        if (other.GetComponent<I_Interactable>() != null)
            other.GetComponent<I_Interactable>().InteractableInRange(); print("interactable in range");
            // trigger in range effect on interactable

        if (PI != null)
            if (PI.IA_Interact_Bark.WasPressedThisFrame())
            {
                if (other.GetComponent<I_Interactable>() != null)
                    other.GetComponent<I_Interactable>().InteractableAction(); // trigger interact effect on interactable
            }
    }
}
