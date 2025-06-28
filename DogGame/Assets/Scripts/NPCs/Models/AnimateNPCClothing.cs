using UnityEngine;
using UnityEngine.Assertions;

// Any object with this script is animated by NPCModel
// should be put on root bones of clothing and should be a chidl object of NPCModel
public class AnimateNPCClothing : MonoBehaviour
{
    private void Awake()
    {
        Assert.IsNotNull(GetComponentInParent<NPCModel>());
    }
}
