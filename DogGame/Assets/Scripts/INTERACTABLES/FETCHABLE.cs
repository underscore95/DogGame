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
    Vector3 vel;
    Vector3 lastFramePos;

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
        if (!fetched)
        {
            bounds = col.bounds;
            scale = bounds.extents.x;
        }

        if (fetched)
        {
            vel = (transform.position - lastFramePos)/Time.deltaTime/2;
            lastFramePos = transform.position;
        }
    }

    public void BeginFetch()
    {
        col.enabled = false;
        rb.isKinematic = true;
        fetched = true;
       // canFetch = false;
        lastFramePos = transform.position;
    }

    public void EndFetch()
    {
        col.enabled = true;
        rb.isKinematic = false;
        fetched = false;
        StartCoroutine(fetchCooldown());
        rb.linearVelocity = vel;
        
    }

    IEnumerator fetchCooldown()
    {
       // canFetch = false;
        yield return new WaitForSeconds(0.4f);
       // canFetch = true;
    }



}
