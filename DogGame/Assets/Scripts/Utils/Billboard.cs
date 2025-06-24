using UnityEngine;

public class Billboard : MonoBehaviour
{
    Quaternion startRotation;
    private void Start()
    {
        startRotation = transform.rotation;
    }

    private void Update()
    {
        //transform.LookAt(-Camera.main.transform.position); // Negative or UI will be flipped
        transform.rotation = Camera.main.transform.rotation * startRotation; 
    }
}
