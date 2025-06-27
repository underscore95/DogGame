using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CAMERATRIGGER : MonoBehaviour
{
    public float time;public bool retriggerable; public GameObject targetObj;
    bool triggered; [SerializeField] private GameObject targetPos;
    public float reenterTime;
    bool canReenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canReenter = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!retriggerable) { canReenter = true; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(canReenter)
            if (retriggerable || !triggered) 
            {
                triggered = true;
                Debug.Log("enteredcameravolume");
                GameObject cam = GameObject.Find("CameraSystem");
                PLAYER_CAMSTATEMACHINE c = cam.GetComponent<PLAYER_CAMSTATEMACHINE>();
                c.StartCutscene(time, targetObj, targetPos.transform);
                StartCoroutine(ReEnterDelay());
            }
        }

        IEnumerator ReEnterDelay()
        {
            canReenter = false;
            yield return new WaitForSeconds(reenterTime);
            canReenter = true;
        }
    }
}
