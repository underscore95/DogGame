using UnityEngine;

public class NPC_BarkReact : MonoBehaviour, I_Interactable
{
    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void InteractableAction()
    {
        animator.SetTrigger("Dance");
        print("dance!!");
        Invoke(nameof(ResetAnimTrigger), 4f);
    }

    void ResetAnimTrigger()
    {
        animator.ResetTrigger("Dance");
    }

    public void InteractableInRange()
    {
    }

    public void OnEnterInteractRange()
    {
    }

    public void OnExitInteractRange()
    {
    }
}
