using UnityEngine;

// An attachment to an NPC model, e.g. hair, skirt, glasses, other accessory
[CreateAssetMenu(fileName = "NewNPCModelAttachment", menuName = "NPC/Model Attachment")]
public class NPCModelAttachment : ScriptableObject 
{
    [SerializeField] private string _boneName;
    [SerializeField] private GameObject _prefab;

    internal void Attach(GameObject baseModel)
    {
        GameObject bone = FindGameObjectInChildrenByName(baseModel, _boneName);
        if (bone == null)
        {
            Debug.LogError($"No bone {_boneName} in model {baseModel.name}, failed to attach an attachment.");
            return;
        }

        MonoBehaviour.Instantiate(_prefab, bone.transform);
        Debug.Log("inst");
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
