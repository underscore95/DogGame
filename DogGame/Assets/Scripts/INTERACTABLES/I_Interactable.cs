using UnityEngine;

public interface I_Interactable
{
    // Called when entering interact range
    void OnEnterInteractRange();

    // Called when exiting interact range
    void OnExitInteractRange();

    // Called every frame while in range
    void InteractableInRange();

    // Called once when interacted with
    void InteractableAction();
}
