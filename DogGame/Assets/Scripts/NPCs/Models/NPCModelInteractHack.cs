using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// accidentally put the NPCModel on a child instead and can't be bothered fixing it for every single npc
public class NPCModelInteractHack : MonoBehaviour, I_Interactable
{
    private List<I_Interactable> _interactables = new();

    private void Awake()
    {
        var found = GetComponentsInChildren<I_Interactable>();
        Assert.IsTrue(found.Length == 2, "invalid amount of interactles (expected 2 (npcmodel and npcmodelinteracthack))");
        foreach (var i in found)
        {
            if (i != this) _interactables.Add(i);
        }
    }

    public void InteractableAction()
    {
        foreach (var i in _interactables)
            i.InteractableAction();
    }

    public void InteractableInRange()
    {
        foreach (var i in _interactables)
            i.InteractableInRange();
    }

    public void OnEnterInteractRange()
    {
        foreach (var i in _interactables)
            i.OnEnterInteractRange();
    }

    public void OnExitInteractRange()
    {
        foreach (var i in _interactables)
            i.OnExitInteractRange();
    }
}
