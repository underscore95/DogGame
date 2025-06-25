using UnityEngine;

public class CAMERATRIGGER : MonoBehaviour
{
    public float time;public bool retriggerable; public GameObject targetObj;
    bool triggered; [SerializeField] private GameObject targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !triggered)
        {
            triggered = true;
            Debug.Log("enteredcameravolume");
            GameObject cam = GameObject.Find("CameraSystem");
           PLAYER_CAMSTATEMACHINE c = cam.GetComponent<PLAYER_CAMSTATEMACHINE>();
            c.StartCutscene(time, targetObj, targetPos.transform);
        }
    }
}
