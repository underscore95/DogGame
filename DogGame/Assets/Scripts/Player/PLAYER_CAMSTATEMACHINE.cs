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
    bool cutscene;
    bool sprint;
    GameObject CameraParent;
    GameObject targetObject;
    Transform targetPos;
    public float endCustsceneTime;
    public float endCustsceneSpd;
    bool cutsceneTransition;
    // Start is called before the first frame update
    void Start()
    {
        CameraParent = Camera.main.transform.parent.gameObject;
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
                WhileCutscene();
                break;
        }
    }

    void WhileCutscene()
    {
        DefaultCameraUpdateLoop();

        if (cutscene)
        {
            CC.DoCutscene(targetPos.transform.position, targetObject.transform, 3f);
        }
        else { 
            if (cutsceneTransition) 
            { CC.EndCutscene(DESIREDPOINT, endCustsceneSpd); } 
            else { AttachCamBack(); }

        }
    }

    IEnumerator EndCutscene()
    {
        cutsceneTransition = true;
        yield return new WaitForSeconds(endCustsceneTime);
        cutsceneTransition = false;
        PM.cutscene = false;
    }

    void AttachCamBack()
    {
        Camera.main.transform.parent = CameraParent.transform; Camera.main.transform.localRotation = Quaternion.identity;
        Camera.main.transform.localPosition = DESIREDPOINT.transform.localPosition;
        Camera.main.transform.localRotation = DESIREDPOINT.transform.localRotation;
        CSS.CStates = PLAYER_CAMERASTATES.CameraStates.Default;
    }

    void DefaultCameraUpdateLoop()
    {
        CC.FollowTarget(CS.CST.hozFollowSpd, DefineVerticalFollowSpeed(), TARGET.transform.position);
        CC.RotateAroundTarget(PI.CamDirection, Gamepad.current == null ? CS.CST.rotateSpdOnMouse : CS.CST.rotateSpd, CS.CST.rotateSmoothness, CS.CST.yRotDivisor, CS.CST.minYRot, CS.CST.maxYRot);
        CC.RotAssistance(PI.InputDirection, CS.CST.rotAssistStr, CS.CST.rotAssistSmooth, PM.speedProg);

        CC.OffsetAssistance(PI.InputDirection, PM.cutscene ? 0 : CS.CST.moveOffsetAmount, CS.CST.moveOffsetSpd, PM.speedProg, DYNAMICOFFSET);
        CC.SlopeRotation(PM.DotUpSlope(transform.forward), PM.currentSlopeAngle, CS.CST.slopeRotSpd);
        if (CSS.CStates != PLAYER_CAMERASTATES.CameraStates.FixedFollow)
        {
            CC.AdjustForCollision(TARGET.transform.position, CS.CST.camDesiredDistance, DESIREDPOINT.transform.position);
        }
        CC.ApplyRot();
        CC.SetFOVAndDist(CS.CST.defaultFOV, CS.CST.FOVChangeSpd, CS.CST.camDesiredDistance, CS.CST.distChangeSpd);
    }

    float DefineVerticalFollowSpeed()
    {
        if (PM.isGrounded) { return PM.lastFrameVel.y < 0 ? CS.CST.downSlopeSpd : CS.CST.vertFollowSpd; }
        else { return PM.lastFrameVel.y < CS.CST.fastFallActivation ? CS.CST.fastFallSpd : CS.CST.jumpSpd; }
    }

    void SetCamDesiredPoint()
    {
        Vector3 pos = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, CS.CST.camDesiredDistance);
        if (Camera.main.transform.parent != null)
        {
            DESIREDPOINT.transform.localPosition = pos;
            DESIREDPOINT.transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            Vector3 posi = DESIREDPOINT.transform.localPosition;
            DESIREDPOINT.transform.localPosition = new Vector3(posi.x, posi.y, CS.DEFAULT_CST.camDesiredDistance);
        }
    }

    public void StartCutscene(float time, GameObject target, Transform targetP)
    {
        PM.cutscene = true;
        targetObject = target;
        targetPos = targetP;
        cutscene = true;
        CSS.CStates = PLAYER_CAMERASTATES.CameraStates.FixedFollow;
        Vector3 pos = Camera.main.transform.position;
        Camera.main.transform.parent = null; 
      //  Camera.main.transform.position = pos;
        StartCoroutine(StopCutscene(time));
    }

    IEnumerator StopCutscene(float time)
    {
        yield return new WaitForSeconds(time);
        cutscene = false;
        StartCoroutine(EndCutscene());
    }

}
