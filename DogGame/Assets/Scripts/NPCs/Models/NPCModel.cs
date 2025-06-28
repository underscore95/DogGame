using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPCModel : MonoBehaviour
{
    [SerializeField] private NPCBone.NPCBodyType _bodyType;
    [SerializeField] private List<NPCModelAttachment> _attachments = new();
    private readonly Dictionary<NPCModelAttachment, GameObject> _attachmentToGameObject = new();

    private void Awake()
    {
        Assert.IsNotNull(_attachments);
        foreach (var attachment in _attachments)
        {
            _attachmentToGameObject.Add(attachment, attachment.Attach(gameObject, _bodyType));
        }
    }

    public bool HasAttachment(NPCModelAttachment attachment)
    {
        return _attachmentToGameObject.ContainsKey(attachment);
    }

    public void RemoveAttachment(NPCModelAttachment attachment)
    {
        Assert.IsTrue(HasAttachment(attachment));
        Destroy(_attachmentToGameObject[attachment]);
        _attachmentToGameObject.Remove(attachment);
    }
}
