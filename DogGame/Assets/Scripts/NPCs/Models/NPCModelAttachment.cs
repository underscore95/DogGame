using UnityEngine;
using UnityEngine.Assertions;

// An attachment to an NPC model, e.g. hair, skirt, glasses, other accessory
[CreateAssetMenu(fileName = "NewNPCModelAttachment", menuName = "NPC/Model Attachment")]
public class NPCModelAttachment : ScriptableObject 
{
    [SerializeField] private NPCBone _bone;
    [SerializeField] private GameObject _prefab;

    internal void Attach(GameObject baseModel, NPCBone.NPCBodyType bodyType)
    {
        string _boneName = _bone.GetBoneName(bodyType);
        GameObject bone = FindGameObjectInChildrenByName(baseModel, _boneName);
        if (bone == null)
        {
            Debug.LogError($"No bone {_boneName} in model {baseModel.name}, failed to attach an attachment. Body type is {bodyType}, is that correct?");
            return;
        }

        MonoBehaviour.Instantiate(_prefab, bone.transform);
    }

    private GameObject FindGameObjectInChildrenByName(GameObject parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform t in parent.transform)
        {
            GameObject go = FindGameObjectInChildrenByName(t.gameObject, name);
            if (go != null) return go;  
        }
        return null;
    }
}
