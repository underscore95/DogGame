using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_STATES : MonoBehaviour
{
    public enum GrndStates {Idle, Walking, Running, Stopping, Skidding };
    public enum AirStates { Jumping, Falling };

    public enum StateGroup {GroundStates, AirStates};

    public GrndStates GState;
    public AirStates AState;
    public StateGroup StateGrp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
