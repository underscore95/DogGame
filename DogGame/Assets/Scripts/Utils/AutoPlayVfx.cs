using UnityEngine;
using UnityEngine.VFX;

public class AutoPlayVfx : MonoBehaviour
{
    private void Start()
    {
        GetComponent<VisualEffect>().Play();
    }
}
