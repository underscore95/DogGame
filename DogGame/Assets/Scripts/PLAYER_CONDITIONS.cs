using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_CONDITIONS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TryingToMove(Vector3 InputDir)
    {
        return InputDir != Vector3.zero;
    }


}
