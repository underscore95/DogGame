using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPCModel : MonoBehaviour
{
    [SerializeField] private NPCBone.NPCBodyType _bodyType;
    [SerializeField] private List<NPCModelAttachment> _attachments = new();

    private void Awake()
    {
        Assert.IsNotNull(_attachments);
        foreach (var attachment in _attachments)
        {
            attachment.Attach(gameObject, _bodyType);
        }
    }
}
