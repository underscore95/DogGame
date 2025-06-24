using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PLAYER_FETCHING : MonoBehaviour
{
    SphereCollider col;
    PLAYER_INPUTS PI;
    GameObject heldObj;
    FETCHABLE heldFetch;
    bool fetching;
    public GameObject holdPoint;
    public Vector3 holdPos;
    public GameObject dropPoint;
    bool grabbedThisFrame;
    bool droppedThisFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PI = GetComponent<PLAYER_INPUTS>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedThisFrame) { return; }
        if (fetching)
        {
            holdPos = holdPoint.transform.position;

            if (PI.IA_Fetch.WasPressedThisFrame())
            {
                heldObj.transform.position = holdPos * heldFetch.scale;
                droppedThisFrame = true;
                heldFetch.EndFetch();
                fetching = false;
            }
        MoveGrabbedObj();
        }
    }

    void DropObject()
    {

    }

    void MoveGrabbedObj()
    {
        heldObj.transform.position = holdPoint.transform.position;
        heldObj.transform.rotation = holdPoint.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (droppedThisFrame) { return; }
        if (other.GetComponent<FETCHABLE>() != null)
        {
            if (PI.IA_Fetch.WasPressedThisFrame() && !fetching)
            {
                heldFetch = other.GetComponent<FETCHABLE>();
                if (!heldFetch.canFetch) { return; }
                heldFetch.BeginFetch();
                heldObj = other.gameObject;
                grabbedThisFrame = true;
                fetching = true;
                }
        }
    }

    private void LateUpdate()
    {
        grabbedThisFrame = false;
        droppedThisFrame = false;
    }



}
