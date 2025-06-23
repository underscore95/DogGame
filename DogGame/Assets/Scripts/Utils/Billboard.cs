using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(-Camera.main.transform.position); // Negative or UI will be flipped
    }
}
