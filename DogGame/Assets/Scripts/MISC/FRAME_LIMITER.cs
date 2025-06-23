using UnityEngine;

public class FRAME_LIMITER : MonoBehaviour
{
    [SerializeField] int FrameRate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FrameRate = Mathf.Clamp(FrameRate, 60, 999);
        Application.targetFrameRate = FrameRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
