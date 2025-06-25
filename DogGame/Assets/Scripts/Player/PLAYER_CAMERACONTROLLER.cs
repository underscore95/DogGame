using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_CAMERACONTROLLER : MonoBehaviour
{

    float xfollow;
    float yfollow;
    float zfollow;
    
    public float freeCamTime;
    bool freeCam;
    float freeCamTimer;
    float desiredDist;
    Vector3 rotEular;
    Vector3 smoothedRot;
    float xAssist;
    Vector3 AssistVec;
    Vector3 dynamicOffset;
    Vector3 sloperot;
    int collidableMask;

    public float occlusionAdjustSpd;
    float zDist;
    bool cameraOccluded;

    // Start is called before the first frame update
    void Start()
    {
        collidableMask = LayerMask.GetMask("Default", "Ground");
    }

    // Update is called once per frame
    void Update()
    {
        FreeCamCountdown();
        
    }

    //Enables a Flag to re-enable Auto-Rotation of the Camera if a Timer reaches 0
    //The Timer resets every time the Camera is moved Automatically
    void FreeCamCountdown()
    {
        freeCamTimer -= Time.deltaTime;
        if (freeCamTimer < 0)
        {
            freeCam = true ;
            freeCamTimer = 0;
        }
        else
        {
            freeCam = false;
        }
    }

    public void SetFOVAndDist(float FOV, float FOVspd, float Dist, float distSpd)
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOV, FOVspd * Time.deltaTime);
        desiredDist = Mathf.Lerp(desiredDist, Dist, distSpd * Time.deltaTime);
    }

    //Detects Walls by Firing Rays between the Player and Camera
    public void AdjustForCollision(Vector3 targetPos, float desiredDistance, Vector3 desiredPoint)
    {
        //Distance and Direction between Player and Camera
        float distance = Vector3.Distance(targetPos, desiredPoint) ;
        Vector3 dir = (targetPos - desiredPoint).normalized;

        //Rays from Player to Cam, and Cam to Player
        bool PlayerToCam = Physics.Raycast(targetPos, -dir, out RaycastHit p2cam, distance, collidableMask, QueryTriggerInteraction.Ignore);
        bool CamToPlayer = Physics.Raycast(desiredPoint, dir, out RaycastHit cam2p, distance, collidableMask, QueryTriggerInteraction.Ignore) ;
        Debug.DrawRay(targetPos, -dir * distance, PlayerToCam ? Color.green : Color.red);
        Debug.DrawRay(desiredPoint, dir * distance, CamToPlayer ? Color.green : Color.red);

        CamToPlayer = CamToPlayer && !cameraOccluded;
        //Position the Camera Wants to move to without any obtrusions
        Vector3 desiredPos = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, desiredDist);
        
        //If a Ray from PlayerToCam hits but to from CamToPlayer, then the Camera is INSIDE an object
        //If they both hit, then there is just a full Object between you and the Camera.
        if (PlayerToCam && !CamToPlayer)
        {
            //Adjust position based on Distance of Raycast
            cameraOccluded = true;
            desiredPos.z = -p2cam.distance + 0.5f;
        }
        else { cameraOccluded = false; }
        //Lerp Z Factor for Smoothness
        zDist = Mathf.Lerp(zDist, desiredPos.z, occlusionAdjustSpd * Time.deltaTime);
        desiredPos.z = zDist;
        Camera.main.transform.localPosition = desiredPos;

    }

    //Follow Target
    public void FollowTarget(float xSpd, float ySpd, Vector3 TARGETtransform)
    {
        xfollow = Mathf.Lerp(xfollow, TARGETtransform.x, xSpd * Time.deltaTime);
        zfollow = Mathf.Lerp(zfollow, TARGETtransform.z, xSpd* Time.deltaTime);
        yfollow = Mathf.Lerp(yfollow, TARGETtransform.y, ySpd * Time.deltaTime);

        transform.position = new Vector3(xfollow, yfollow, zfollow);
    }

    public void RotateAroundTarget(Vector3 input, float rotSpeed, float rotSmooth, float yDivision, float minYRot, float maxYRot)
    {
        //Camera input has been moved, increase the free-cam timer
        if  (input.y != 0 && freeCamTimer == 0) { freeCamTimer += freeCamTime; }

        //Camera Rotation speed altered on the Vertical Axis (ideally slower) 
        input.x *= yDivision;
        //Add inputs to Camera Rotation
        rotEular += (input * rotSpeed * Time.deltaTime);
        rotEular.x = Mathf.Clamp(rotEular.x, minYRot, maxYRot);

        //Rotation is smoothed
        //Rotation is clamped so you can't move the Camera upside down
        smoothedRot = Vector3.Lerp(smoothedRot, rotEular, rotSmooth * Time.deltaTime);
        smoothedRot.x = Mathf.Clamp(smoothedRot.x, minYRot, maxYRot);
    }

    //Auto-Rotate Camera to the Slope of the Ground
    public void SlopeRotation(float dot, float angle, float spd)
    {
        //Dot product of the Camera Projected on the Ground
        //This will get the Camera's rotation from the Up and Down Direction of the slope, presented from -1 to 1
        float amount = Mathf.Abs( angle * dot);

        //Don't rotate the Camera if we are going Up a Slope, as visibility isn't affected by this
        //When going down a slope, the Camera will Rotate to the Angle * 1.5f
        if (dot > 0) { amount = 0;} else { amount = Mathf.Abs(angle * dot) * 1.5F;}

        //Smooth Rotate towards the calculated value
        sloperot = Vector3.zero;
        sloperot.x = amount;
        AssistVec = Vector3.Lerp(AssistVec, sloperot, spd * Time.deltaTime);

    }

    //Dynamically moves Camera in direction of Movement
    public void OffsetAssistance(Vector3 inputDir, float amount, float speed, float speedProg, GameObject obj)
    {
        if (inputDir.y < 0)
        {
            inputDir.y *= 0.5f;
        }
        //Uses Input Direction to Adjust Offset
        Vector3 location = inputDir * amount;
        dynamicOffset = Vector3.Lerp(dynamicOffset, location, speed * Time.deltaTime);
        obj.transform.localPosition = dynamicOffset;
    }


    //Adjust the Rotation of the Camera based on Movement input (QOL)
    public void RotAssistance(Vector3 inputDir, float strength, float smoothing, float speedProg)
    {
        if (inputDir != Vector3.zero && freeCam)
        {   //Dot-product of the Inputs closeness to the Left or Right axis
            float rightDot = Vector3.Dot(Vector3.right, inputDir);
            float leftDot = Vector3.Dot(Vector3.left, inputDir);
            float amount;
            if (Mathf.Abs(rightDot) > Mathf.Abs(leftDot)) { amount = rightDot; } else { amount = leftDot; }

            //Multiply by Strength, as well as the Player's velocity represented from 0-1
            amount *= -strength * Time.deltaTime * speedProg;
            //Add this to horizontal rotation
            xAssist = amount;
            rotEular.y += xAssist;
        }
    }

    public void ApplyRot()
    {
        transform.rotation = Quaternion.Euler(smoothedRot + AssistVec);

    }

    public void CollisionDetection()
    {

    }

    

    
}
