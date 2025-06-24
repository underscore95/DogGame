using System.Collections;
using UnityEngine;

public class FETCHABLE : MonoBehaviour
{
    public bool fetched;
    public bool canFetch = true;
    Collider col;
    Rigidbody rb;
    public float scale;
    Bounds bounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFetch = true;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bounds = col.bounds;
        scale = bounds.extents.x;
    }

    public void BeginFetch()
    {
        col.enabled = false;
        rb.isKinematic = true;
        fetched = true;
        canFetch = false;
    }

    public void EndFetch()
    {
        col.enabled = true;
        rb.isKinematic = false;
        fetched = false;
        StartCoroutine(fetchCooldown());
    }

    IEnumerator fetchCooldown()
    {
        canFetch = false;
        yield return new WaitForSeconds(0.4f);
        canFetch = true;
    }



}
