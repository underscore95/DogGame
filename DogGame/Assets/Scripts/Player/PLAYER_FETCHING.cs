using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PLAYER_FETCHING : MonoBehaviour
{
   
    PLAYER_INPUTS PI;

    public GameObject heldObj;
    public FETCHABLE heldFetch;
    bool grabInput;
    public bool fetching;
    public GameObject holdPoint;
    public GameObject dropPoint;
    Vector3 holdPos;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PI = GetComponent<PLAYER_INPUTS>();
    }

    //Keeps input enabled for a few seconds
    IEnumerator InputBuffer()
    {
        grabInput = true;
        yield return new WaitForSeconds(0.3f);
        grabInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PI.IA_Fetch.WasPressedThisFrame())
        {
            StartCoroutine(InputBuffer());
        }

        if (fetching)
        {
            //Drop Object is grab button is pressed while grabbing
            if (grabInput) { DropObject(); grabInput = false; }
            MoveGrabbedObj();
        }
    }

    public void DropObject()
    {  
        heldObj.transform.position = dropPoint.transform.position;
        heldFetch.EndFetch();
        fetching = false;   
      //  heldFetch = null;
      //  heldObj = null;
    }

    void MoveGrabbedObj()
    {
        if (heldObj == null) return;
        holdPos = holdPoint.transform.position;
        heldObj.transform.SetPositionAndRotation(holdPoint.transform.position, holdPoint.transform.rotation);
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<FETCHABLE>() != null)
        {
            if (grabInput)
            {
                heldFetch = other.GetComponent<FETCHABLE>();
                if (!heldFetch.canFetch) { return; }
                heldFetch.BeginFetch();
                heldObj = other.gameObject;
                fetching = true;
                grabInput = false;
            }
        }
    }

  

   

    



}
