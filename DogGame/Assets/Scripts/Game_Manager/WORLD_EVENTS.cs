using UnityEngine;

public class WORLD_EVENTS : MonoBehaviour
{
    [SerializeField] public GameObject Bridge;
    [SerializeField] public GameObject BridgeRemoveColliders;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoIHaveABridge()
    {
        Debug.Log(Bridge);
        Bridge.SetActive(true);
        BridgeRemoveColliders.SetActive(false);
    }
}
