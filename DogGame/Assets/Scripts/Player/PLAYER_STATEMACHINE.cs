using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PLAYER_STATEMACHINE : MonoBehaviour
{
    PLAYER_STATES PSS;
    PLAYER_STATS PS;
    PLAYER_MOVEMENT PM;
    PLAYER_INPUTS PI;
    PLAYER_CONDITIONS PC;
    bool attemptMove;
    bool jbuffer;
    // Start is called before the first frame update
    void Start()
    {

        PSS = GetComponent<PLAYER_STATES>();
        PS = GetComponent<PLAYER_STATS>();
        PM = GetComponent<PLAYER_MOVEMENT>();
        PI = GetComponent<PLAYER_INPUTS>();
        PC = GetComponent<PLAYER_CONDITIONS>();
        PM.ApplyJump(Vector3.up, 0f, PS.AST.airMoveStr, PS.AST.jumpVelModifier, false, PS.AST.airDragStr, PS.AST.heaviness);
        PM.speedTarget = PS.GST.maxSpeed;
        PM.airMoveMaxSpd = PS.GST.maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.9)
        {


            bool attemptMove = PC.TryingToMove(PI.InputDirection);

            switch (PSS.StateGrp)
            {
                case PLAYER_STATES.StateGroup.GroundStates:
                    GroundUpdateLoop();
                    break;
                case PLAYER_STATES.StateGroup.AirStates:
                    AirUpdateLoop();
                    break;
            }
        }
    }
    
    

    void GroundUpdateLoop()
    {

        if (!LockedGrndMovementState())
        {
            RegularMovementLoop();
        }
        else
        {
            PM.ApplyMovement(true);
            PM.Accelerate(PS.GST.skidStr, 0f);
            if (PM.speedProg == 0)
            { PSS.GState = PLAYER_STATES.GrndStates.Idle; }
        }
        //UNIQUE STATE CASES

    }

    void RegularMovementLoop()
    {
        bool attemptMove = PC.TryingToMove(PI.InputDirection);
        bool attemptJump = PI.IA_Jump.WasPerformedThisFrame();
        bool attempSprint = PI.IA_Sprint.WasPerformedThisFrame();
        bool sprinting = PSS.GState == PLAYER_STATES.GrndStates.Running;
        if (attempSprint)
        {
            if (PSS.GState == PLAYER_STATES.GrndStates.Running) { PSS.GState = PLAYER_STATES.GrndStates.Idle; }
            else if (PM.CanSprint(PS.GST.minSprintActivation))
            {
                PSS.GState = PLAYER_STATES.GrndStates.Running;
            }
        }

        if (sprinting && PI.InputDirection == Vector3.zero)
        {
            PSS.GState = PLAYER_STATES.GrndStates.Walking;
        }

        bool lerpOrslerp = !sprinting;
        float turnSpd = sprinting ? PS.GST.sprintTurnSpd : PS.GST.turnSpd;
        float moveSpd = PSS.GState != PLAYER_STATES.GrndStates.Running ? PS.GST.maxSpeed : PS.GST.sprintMaxSpeed;
        if (PM.cutscene) { attemptMove = false; } 
        if (!PM.isGrounded)
        { PSS.StateGrp = PLAYER_STATES.StateGroup.AirStates; PSS.AState = PLAYER_STATES.AirStates.Falling; PM.ApplyMovement(false); }

        else if (attemptJump || PM.JumpBuffer)
        {
            if (!PM.cutscene) 
            {
                Jump();
            }
           
        }
        else
        {
            PM.CheckGround();
            if (PM.IsSkidding(PI.InputDirection, PS.GST.minSkidAngle, PS.GST.minSkidVel) && sprinting)
            { PSS.GState = PLAYER_STATES.GrndStates.Walking; Debug.Log("skid!"); 
            }
            else
            {
                PSS.GState = MovementState();
                PM.Accelerate(attemptMove ? PS.GST.accelSpd : PM.cutscene ? 50f : PS.GST.decelSpd, attemptMove ? moveSpd : 0);
                if (!PM.cutscene)
                {
                    PM.LerpTurn(PI.InputDirection, turnSpd, PS.GST.minTurnVel, true, true, lerpOrslerp);
                }
                PM.ApplyMovement(true);
            }
        }
    }

    PLAYER_STATES.GrndStates MovementState()
    {
        if (PM.cutscene) { return PLAYER_STATES.GrndStates.Suncream; }
        if (PSS.GState == PLAYER_STATES.GrndStates.Running) { return PLAYER_STATES.GrndStates.Running; }

        if (PM.speedProg < 0.1)
            return PLAYER_STATES.GrndStates.Idle;
        else if (PM.speedProg > 0.1)
            return PLAYER_STATES.GrndStates.Walking;
        else
            return PLAYER_STATES.GrndStates.Idle;
    }

    bool LockedGrndMovementState()
    {
        return PSS.GState == PLAYER_STATES.GrndStates.Skidding;
    }

    void AirUpdateLoop()
    {
        bool attemptMove = PC.TryingToMove(PI.InputDirection);
        bool attemptJump = PI.IA_Jump.WasPerformedThisFrame();
        PM.LerpTurn(PI.InputDirection, PS.GST.turnSpd, PS.GST.minTurnVel, true, true, true);


        if (attemptJump)
        { PM.StartCoroutine(PM.StartJumpBuffer(PS.AST.jumpBufferTime)); }


        if (attemptMove)
            { PM.ApplyAirMovement(PI.InputDirection, true);}
        else
            {PM.ApplyAirDrag();}

        PM.ApplyMovement(false);
        if (PM.isGrounded)
        { PM.OnGroundEnter(); { PSS.StateGrp = PLAYER_STATES.StateGroup.GroundStates; } }
        else
        {
            switch (PSS.AState)
            {
                case PLAYER_STATES.AirStates.Jumping:
                    if (!PI.IA_Jump.IsPressed())
                    {
                        if (PM.CutOffJump(PS.AST.jumpCutOffStr, PS.AST.jumpMinCutOffVel, PS.AST.jumpMaxCutOffVel))
                        { PSS.AState = PLAYER_STATES.AirStates.Falling; }
                    }
                    if (PM.IsFalling())
                    { PSS.AState = PLAYER_STATES.AirStates.Falling; }
                    break;

                case PLAYER_STATES.AirStates.Falling:
                    PM.CheckGround();
                    break;
            }
        }
    }

    void Jump()
    {
        PM.StartCoroutine(PM.UnstickGroundTimer());
        PM.ApplyJump(Vector3.up, PS.AST.jumpStr, PS.AST.airMoveStr, 1f, false, PS.AST.airDragStr, PS.AST.heaviness);
        //PM.ApplyMovement(false);
        PSS.StateGrp = PLAYER_STATES.StateGroup.AirStates;
        PSS.AState = PLAYER_STATES.AirStates.Jumping;
    }
}
