using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_STATS : MonoBehaviour
{
    [System.Serializable]
    public struct GrndStats
    {
        public float maxSpeed;
        public float accelSpd;
        public float decelSpd;
        public float turnSpd;
        public float minTurnVel;
        public float angDrag;
        public float angDragMinAngle;
        public float angDragMinVel;
        public float minSkidVel;
        public float minSkidAngle;
        public float skidStr;
        public float sprintMaxSpeed;
        public float minSprintActivation;
        public float sprintTurnSpd;


    }

    [System.Serializable]
    public struct AirStats
    {
        public float airMoveStr;
        public float airDragStr;
        public float jumpStr;
        public float jumpVelModifier;
        public float heaviness;
        public float jumpBufferTime;

        public float jumpCutOffStr;
        public float jumpMaxCutOffVel;
        public float jumpMinCutOffVel;
    }

    [SerializeField]
    public GrndStats GST;
    [SerializeField]
    public AirStats AST;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
