using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCBone", menuName = "NPC/NPCBone")]
public class NPCBone : ScriptableObject
{
    public enum NPCBodyType
    {
        // if adding value, make sure to add bone name in all the scriptable objs
        Male, Female, ShirtGuy, SunglassesLady, HatGuy
    }

    [Serializable]
    public struct BodyTypeBone
    {
        public NPCBodyType Type;
        public string Name;
    }

    [SerializeField] private List<BodyTypeBone> _bones = new();

    private void OnValidate()
    {
        var found = new HashSet<NPCBodyType>();
        for (int i = _bones.Count - 1; i >= 0; i--)
        {
            var bone = _bones[i];
            if (found.Contains(bone.Type))
            {
                _bones.RemoveAt(i);
            }
            else
            {
                found.Add(bone.Type);
            }
        }

        foreach (NPCBodyType type in Enum.GetValues(typeof(NPCBodyType)))
        {
            if (!found.Contains(type))
            {
                _bones.Add(new BodyTypeBone { Type = type, Name = string.Empty });
            }
        }
    }

    public string GetBoneName(NPCBodyType type)
    {
        foreach (var bone in _bones)
        {
            if (bone.Type == type)
                return bone.Name;
        }
        Debug.LogError($"No bone defined for body type {type}");
        return string.Empty;
    }
}
