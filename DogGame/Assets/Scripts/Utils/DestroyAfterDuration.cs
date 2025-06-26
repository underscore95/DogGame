using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyVfx : MonoBehaviour
{
    [SerializeField] private float _duration = 5;
    private void Awake()
    {
        StartCoroutine(DestroyLater());
    }

    private IEnumerator DestroyLater()
    {
        yield return new WaitForSeconds( _duration );
        Destroy(gameObject);
    }
}
