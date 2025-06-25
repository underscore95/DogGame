using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLAYER_CAMSTATEMACHINE : MonoBehaviour
{
    PLAYER_CAMERACONTROLLER CC;
    PLAYER_CAMERASTATES CSS;
    PLAYER_CAMERASTATS CS;
    [SerializeField] GameObject TARGET;
    [SerializeField] GameObject DYNAMICOFFSET;
    [SerializeField] GameObject DESIREDPOINT;
  
    PLAYER_MOVEMENT PM;
    PLAYER_INPUTS PI;
    PLAYER_STATES PS;

    bool sprint;
    // Start is called before the first frame update
    void Start()
    {
        DESIREDPOINT = GameObject.Find("CamDesiredPoint");
        DYNAMICOFFSET = GameObject.Find("DynamicOffset");
        CC = GetComponent<PLAYER_CAMERACONTROLLER>();
        CSS = GetComponent<PLAYER_CAMERASTATES>();
        CS = GetComponent<PLAYER_CAMERASTATS>();
        PM = TARGET.GetComponent<PLAYER_MOVEMENT>();
        PI = TARGET.GetComponent<PLAYER_INPUTS>();
        PS = TARGET.GetComponent<PLAYER_STATES>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PS.GState == PLAYER_STATES.GrndStates.Running)
        { if (!sprint) { CS.CST = CS.SPRINT_CST; sprint = true; } }
        else
        { if (sprint) { CS.CST = CS.DEFAULT_CST; sprint = false; } }    
           

        SetCamDesiredPoint();
        switch (CSS.CStates)
        {

            case PLAYER_CAMERASTATES.CameraStates.Default:
                DefaultCameraUpdateLoop(); break; 
            case PLAYER_CAMERASTATES.CameraStates.FixedFollow:
                break;
        }
    }

    void DefaultCameraUpdateLoop()
    {
        CC.FollowTarget(CS.CST.hozFollowSpd, DefineVerticalFollowSpeed(), TARGET.transform.position);
        CC.RotateAroundTarget(PI.CamDirection, Gamepad.current == null ? CS.CST.rotateSpdOnMouse : CS.CST.rotateSpd, CS.CST.rotateSmoothness, CS.CST.yRotDivisor, CS.CST.minYRot, CS.CST.maxYRot);
        CC.RotAssistance(PI.InputDirection, CS.CST.rotAssistStr, CS.CST.rotAssistSmooth, PM.speedProg);
        CC.OffsetAssistance(PI.InputDirection, CS.CST.moveOffsetAmount, CS.CST.moveOffsetSpd, PM.speedProg, DYNAMICOFFSET);
        CC.SlopeRotation(PM.DotUpSlope(transform.forward), PM.currentSlopeAngle, CS.CST.slopeRotSpd);
        CC.AdjustForCollision(TARGET.transform.position, CS.CST.camDesiredDistance, DESIREDPOINT.transform.position);
        CC.ApplyRot();
        CC.SetFOVAndDist(CS.CST.defaultFOV, CS.CST.FOVChangeSpd, CS.CST.camDesiredDistance, CS.CST.distChangeSpd) ;
    }

    float DefineVerticalFollowSpeed()
    {
        if (PM.isGrounded) { return PM.lastFrameVel.y < 0 ? CS.CST.downSlopeSpd : CS.CST.vertFollowSpd; }
        else { return PM.lastFrameVel.y < CS.CST.fastFallActivation ? CS.CST.fastFallSpd : CS.CST.jumpSpd; }
    }

    void SetCamDesiredPoint()
    {
        Vector3 pos = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, CS.CST.camDesiredDistance); 
        DESIREDPOINT.transform.localPosition = pos;
        DESIREDPOINT.transform.rotation = Camera.main.transform.rotation;
    }
}
