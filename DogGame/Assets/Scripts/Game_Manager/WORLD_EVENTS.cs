using UnityEngine;

public class WORLD_EVENTS : MonoBehaviour
{
    [SerializeField] public GameObject Bridge;
    [SerializeField] public GameObject BridgeRemoveColliders;
    [SerializeField] GameObject Sign;
    [SerializeField] GameObject SignViewPos;
    [SerializeField] float SignCutsceneTime;
    GameObject player;
    GameObject camsystem;
    PLAYER_CAMSTATEMACHINE cs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camsystem = GameObject.Find("CameraSystem");    
        cs = camsystem.GetComponent<PLAYER_CAMSTATEMACHINE>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoIHaveABridge()
    {
       
    }

    public void SignCutscene()
    {
        cs.StartCutscene(2.5f, Sign, SignViewPos.transform);
    }
}
