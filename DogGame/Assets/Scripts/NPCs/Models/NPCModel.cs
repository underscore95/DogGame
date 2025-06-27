using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPCModel : MonoBehaviour
{
    [SerializeField] private List<NPCModelAttachment> _attachments;

    private void Awake()
    {
        foreach (var attachment in _attachments)
        {
            attachment.Attach(gameObject);
        }
    }
}
