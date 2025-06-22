using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmoveobj : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = vel;
        vel = rb.linearVelocity;
    }
}
