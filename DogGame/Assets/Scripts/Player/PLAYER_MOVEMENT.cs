using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PLAYER_MOVEMENT : MonoBehaviour
{
    Vector3 moveAwayDir;
    Vector3 dirNoZero;
    public Vector3 velocity;
    public Vector3 normalisedVel;

    [SerializeField]
    public float speedTarget;


    float gravityStr;
    public bool JumpBuffer;

    bool GroundUnstick;

    #region Velocity Inheritance
    Vector3 addedVel;
    
    #endregion

    #region Applying Velocity
    Vector3 moveAmount;
    Vector3 gravityPass;
    [SerializeField]
    float maxBounces;
    #endregion

    #region Velocity and Direction Values
    Vector3 desiredDir;
    Vector3 finalDir;
    public Vector3 targetDir;
     
    [SerializeField]
    float moveSpd;
    [SerializeField]
    public float speedProg;
    public float grav;
    Vector3 overlapFixVec;
    Vector3 colfloatVec;

    #endregion

    #region Previous Frame Velocity Values
    Vector3 lastFramePos;
    public Vector3 lastFrameVel;
    Vector3 prevAngularVel;
    Vector3 prevInputDir;
    #endregion

    #region Collider Bounds Info 
    CapsuleCollider col;
    Bounds bounds;
    Vector3 currentBoundsSize;
    float colHalfSize;
    public Vector3 p1;
    public Vector3 p2;
    [SerializeField]
    float skinWidth;
    #endregion

    #region Ground Detection Values
    public bool isGrounded;
    bool[] GroundCasts = new bool[5];
    RaycastHit[] GroundCastInfos = new RaycastHit[5];
    Vector3[] CastOffsets = new Vector3[5];
    public RaycastHit WinningGroundCast;
    int WinningCastIndex;
    int collidableMask;
    int walkableMask;


    public float airDragStr;
    public float airMoveSpd;
    public float airMoveMaxSpd;

    [SerializeField]
    float colRideHeight;
    [SerializeField]
    float groundStickLength;
    [SerializeField]
    float airStickLength;

    [SerializeField]
    public float maxSlopeAngle;
    [SerializeField]
    public float currentSlopeAngle;
    #endregion

    #region Debug Values
    public bool debug;
    Vector3 DEBUG_colOverlapPoint;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
       // layerMasks = LayerMask.GetMask("Ground");
        collidableMask = LayerMask.GetMask("Ground", "Default");
        walkableMask = LayerMask.GetMask("Ground", "Default");
        SetColliderOffets();
       
        
    }

    // Update is called once per frame
    void Update()
    {
      
        lastFramePos = transform.position;
        StickToGround();

        SetColliderBounds();
       
        CollisionDetection();

        
    }

   

    public Vector3 GetSlopeDownwardsVector()
    {
        return Vector3.ProjectOnPlane(WinningGroundCast.normal, Vector3.down);
    }

    public float DotUpSlope(Vector3 vec)
    {
        vec = Vector3.ProjectOnPlane(vec, WinningGroundCast.normal);
        return Vector3.Dot(vec, -GetSlopeDownwardsVector());
    }


   public IEnumerator StartJumpBuffer(float time)
    {
        JumpBuffer = true;
        yield return new WaitForSeconds(time);
        JumpBuffer = false;
    }

    public void CheckGround()
    {
        if (!GroundUnstick)
        {


            float length = isGrounded ? groundStickLength * colHalfSize : lastFrameVel.y < 0.1 ? airStickLength * colHalfSize : 0;

            bool groundHit = false;
            float dist = Mathf.Infinity;
            //Cast from Middle and Four Corners of Player
            for (int i = 0; i < 5; i++)
            {
                GroundCasts[i] = Physics.Raycast(transform.position + CastOffsets[i], -transform.up, out GroundCastInfos[i], length, walkableMask, QueryTriggerInteraction.Ignore);
                bool hit = GroundCasts[i] && Vector3.Angle(Vector3.up, GroundCastInfos[i].normal) < maxSlopeAngle;
                Debug.DrawRay(transform.position + CastOffsets[i], Vector3.down * length, hit ? Color.green : Color.red);

                if (hit)
                {
                    if (i == 0)
                    {
                        WinningGroundCast = GroundCastInfos[i]; WinningCastIndex = i; groundHit = true; dist = WinningGroundCast.distance; currentSlopeAngle = Vector3.Angle(Vector3.up, WinningGroundCast.normal);
                    }
                    else if (GroundCastInfos[i].distance < dist)
                    {
                        WinningGroundCast = GroundCastInfos[i]; WinningCastIndex = i; groundHit = true; dist = WinningGroundCast.distance; currentSlopeAngle = Vector3.Angle(Vector3.up, WinningGroundCast.normal);
                    }
                }
            }

            bool hit2 = Physics.Raycast(transform.position, -transform.up, out GroundCastInfos[0], length, walkableMask, QueryTriggerInteraction.Ignore);
            isGrounded = hit2 && Vector3.Angle(Vector3.up, GroundCastInfos[0].normal) < maxSlopeAngle;
            WinningGroundCast = GroundCastInfos[0];
            if (WinningGroundCast.rigidbody != null)
            {
                addedVel = WinningGroundCast.rigidbody.linearVelocity;
            }
            else { addedVel = Vector3.zero; }
        }
    }

    


   

    
   
   

   
    

    #region Finished Code (For now)
    //Sticks the Collider to the Ground based on a Set Height - DONE
    void StickToGround()
    {
        if (isGrounded && !GroundUnstick)
        {
            float posFromGround = WinningGroundCast.point.y + colRideHeight * bounds.extents.y;
            float amount = posFromGround - transform.position.y;
            //Calculating the Vertical difference needed to Float Collider up.
            //This is then applied but using CollideAndSlide to avoid Clipping.
            Vector3 vec = amount * Vector3.up;
            transform.position += CollideAndSlide(vec, transform.position, 0, vec, false);
            CollisionDetection();
        }
    }

    //Collider Offsets need to be Reset if the Size of the Player has changed. - DONE
    void SetColliderOffets()
    {
        currentBoundsSize = bounds.extents;
        colHalfSize = currentBoundsSize.y;
        float radius = currentBoundsSize.x;
        CastOffsets[0] = Vector3.zero;
        CastOffsets[1] = new Vector3(0, 0, radius);
        CastOffsets[2] = new Vector3(0, 0, -radius);
        CastOffsets[3] = new Vector3(radius, 0, 0);
        CastOffsets[4] = new Vector3(-radius, 0, 0);

    }

    //DONE
    Vector3 ProjectVecToCamera(Vector3 velocity)
    {
        Vector3 camFwd = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camFwd.y = 0f;
        camRight.y = 0f;
        camFwd.Normalize();
        camRight.Normalize();

        return velocity.z * camFwd + velocity.x * camRight;
    }

    //Fires an Overlap around the Player capsule to detect Overlaps and move out of them. - DONE
    //Typically not needed because of Collide and Slide collision response, but useful for dynamic objects moving towards an idling Player
    void CollisionDetection()
    {

        SetColPoints(transform.position);
        Vector3 moveAwayAmount;

        //Checks each Overlap hit
        var overlaps = Physics.OverlapCapsule(p1, p2, bounds.extents.x, collidableMask, QueryTriggerInteraction.Ignore);
        foreach (var overlap in overlaps)
        {
            //Gets the closest point on the Bounds of the overlapping object to calculate the distance.
            // Vector3 overlapPoint = overlap.ClosestPoint(transform.position);
            // if (overlapPoint == transform.position)
            // { overlapPoint = overlap.ClosestPointOnBounds(transform.position); }
            Vector3 overlapPoint = overlap.ClosestPointOnBounds(transform.position);
            DEBUG_colOverlapPoint = overlapPoint;
            float dist = Vector3.Distance(transform.position, overlapPoint);

            

            RaycastHit playerToPoint;
            RaycastHit pointToPlayer;
            //Fires a Raycast from Point to Player, and from Player to Point, to detect the Overlap point and obtain a Normal Vector for it.
            //Two are necessary as Colliders are only detected One-Way, so depending on how deep the Overlap is, both must be tested.

            bool cast1 = Physics.Raycast(transform.position, overlapPoint - transform.position, out playerToPoint, dist + 2f, collidableMask, QueryTriggerInteraction.Ignore);
            bool cast2 = Physics.Raycast(overlapPoint, overlapPoint - overlapPoint, out pointToPlayer, dist + 2f, collidableMask, QueryTriggerInteraction.Ignore);

            Debug.DrawRay(transform.position, (overlapPoint - transform.position) * dist, cast1 ? Color.green : Color.red);
            Debug.DrawRay(overlapPoint, (transform.position - overlapPoint) * dist, cast1 ? Color.green : Color.red);

            //The direction the Player will move out of the Collision is based on the Normal Vector of the Collision Point
            moveAwayDir = cast1 ? playerToPoint.normal : pointToPlayer.normal;
            //Calculates the Length of the Vector needed to Move out of the collision. This needs the Distance between the Collision and Player, as well as the exact dimensions of the Player Collider
            //For example, a Collision of Distance 3 will mean different amounts are needed to move away all based on the size of the Player Collider.
            moveAwayAmount = new Vector3(bounds.extents.x - dist + skinWidth, bounds.extents.y - dist + skinWidth, bounds.extents.z - dist + skinWidth);

            //Decides based on the Angle of Collision whether to Move the Player away Vertically or Horizontally, as both causes them to move too far out (COULD BE REFINED)
            if (Mathf.Abs(moveAwayDir.y) > 0.45)
            {
                moveAwayAmount.x = 0;
                moveAwayAmount.z = 0;
            }
            else
            {
                moveAwayAmount.y = 0;
            }
            //Apply to Player!
            if (overlapPoint != transform.position) 
            {
                overlapFixVec = Vector3.Scale(moveAwayAmount, moveAwayDir);
                transform.position += overlapFixVec;
            }
            else
            { Debug.Log("failed overlap"); }
            

        }




    }

   


    //Lerp to Desired Direction of Movement
    public void LerpTurn(Vector3 desiredDir, float turnSpd, float minActivation, bool ProjectToCam, bool ProjectToGround)
    {
        //Project the Input Direction to the Camera if wanted
        Vector3 dir = ProjectToCam ? ProjectVecToCamera(desiredDir) : desiredDir;
        //Likewise with the Ground Normal
        dir = ProjectToGround && isGrounded ? Vector3.ProjectOnPlane(dir, WinningGroundCast.normal) : dir;

        //Lerp towards direction of Movement
        if (dir != Vector3.zero)
        {
            targetDir = dir;
            //At slow speeds, Movement will be more accurate to Input Direction
            if (speedProg < minActivation)
            { finalDir = dir; }
            finalDir = Vector3.Lerp(finalDir, dir, turnSpd * Time.deltaTime);
            dirNoZero = finalDir;
        }
    }

    //DONE
    public Vector3 ApplyGravity(Vector3 direction, float strength)
    {
        return (direction * strength * Time.deltaTime);
    }

    //Expands Bounds of Collider by SkinWidth - DONE
    void SetColliderBounds()
    {
        bounds = col.bounds;
        bounds.Expand(-2 * skinWidth);
        //Updates the Offsets of the Ground Check, to make sure they work at the changed size
        if (bounds.extents != currentBoundsSize)
        { SetColliderOffets(); }
    }

    //Sets Point1 and Point2 of the Capsule Collider, for Overlap tests - DONE
    void SetColPoints(Vector3 pos)
    {
        p1 = pos + (Vector3.up * ((bounds.extents.y - bounds.extents.x)));
        p2 = pos - (Vector3.up * ((bounds.extents.y - bounds.extents.x)));
    }

    //DONE
    public bool IsSkidding(Vector3 InputDir, float minAngle, float minVel)
    {
        //Check the Angle of Input compared to that of Current Movement to allow beginning a Skid State
        return Vector3.Angle(dirNoZero, InputDir) > minAngle && speedProg > minVel;
    }

    //DONE
    public void Accelerate(float amount, float target)
    {
      
        //Increase/Decrease MoveSpd per-frame
        moveSpd = Mathf.MoveTowards(moveSpd, target, amount * Time.deltaTime);
        //Clamped to stop MoveSpd becoming a negative as this will reverse controls.
        moveSpd = Mathf.Clamp(moveSpd, 0, Mathf.Infinity);

        if (target != 0) {speedTarget =  target; }  
        speedProg = moveSpd / speedTarget;
    }

    //Collision Response algorithm to Slide Player across Collision Points when Moving - DONE
    Vector3 CollideAndSlide(Vector3 velocity, Vector3 pos, int depth, Vector3 velInit, bool gravPass)
    {
        if (depth >= maxBounces)
        { return new Vector3(0, 0, 0); }
        SetColPoints(pos);

        float dist = velocity.magnitude + skinWidth;
        RaycastHit hitInfo;

        //Capsule Cast in the Direction of Velocity
        bool hit = Physics.CapsuleCast(p1, p2, bounds.extents.x, velocity.normalized, out hitInfo, dist, collidableMask, QueryTriggerInteraction.Ignore);

        if (hit)
        {
           // if (hitInfo.distance < skinWidth)
           // { return new Vector3(0, velocity.y, 0); }

            //If Collider detected is that of a Dynamic Rigidbody, add a Force to Push the Object based on Velocity
            //Only do this at Depth 0, or multiple Forces will be added per-frame.
            if (hitInfo.collider.attachedRigidbody && depth == 0 && hitInfo.collider.attachedRigidbody.isKinematic != true)
            {
                hitInfo.collider.attachedRigidbody.AddForceAtPosition(velInit, hitInfo.point, ForceMode.Force);
            }

         



            //Gets the exact Distance to Snap Player to the Point of Collision
            Vector3 snapToSurface = velocity.normalized * (hitInfo.distance - skinWidth);
            //The Leftover Vector is whatever is left of this
            Vector3 leftover = velocity - snapToSurface;
            
            float angle = Vector3.Angle(Vector3.up, hitInfo.normal);
            float mag = leftover.magnitude;

            //If this Ground is Walkable, Project the Leftover Vector onto the surface
            if (angle < maxSlopeAngle || !gravPass)
            {
                leftover = Vector3.ProjectOnPlane(leftover, hitInfo.normal).normalized;
                leftover *= mag;
            }
            else
            //If this Ground is not Walkable, do the same, but remove the Y Factor altogether so you can't climb up extreme Slopes.
            {
                //The Vector is scaled based on your Direction towards the wall
                float scale = 1 - Vector3.Dot(
                 new Vector3(hitInfo.normal.x, 0, hitInfo.normal.z).normalized,
                 -new Vector3(velInit.x, 0, velInit.z).normalized);
                leftover = Vector3.ProjectOnPlane(leftover, hitInfo.normal).normalized;
                leftover *= mag;
                leftover *= scale;
            }
            //Recurse the algorithm until MaxBounces is reached, or no Collisions are detected.
            return snapToSurface + CollideAndSlide(leftover, pos + snapToSurface, depth + 1, velInit, gravPass);
        }
        return velocity;

    }

    #endregion



    public void ApplyMovement(bool grounded)
    {
        
        //When Grounded, use the Direction of Movement scaled by MoveSpd. When not Grounded, use an additive on-top of Previous Frame Velocity. This will have Movement, Drag and Gravity added on.
        velocity = grounded ? (finalDir * moveSpd) : lastFrameVel += ApplyGravity(Vector3.down, gravityStr);
        //Collide and Slide Movement
        float length = velocity.magnitude * Time.deltaTime;
        lastFrameVel = CollideAndSlide((velocity + addedVel) * Time.deltaTime, transform.position, 0, velocity + addedVel, grounded);
       // lastFrameVel = Vector3.ClampMagnitude(lastFrameVel, length);
        transform.position += lastFrameVel;
        CollisionDetection();
        lastFrameVel /= Time.deltaTime;

    }




    public void ApplyAirMovement(Vector3 inputDir, bool ProjectToCam)
    {
        Vector3 dir = ProjectToCam ? ProjectVecToCamera(inputDir) : inputDir;
        dir *= airMoveSpd;
        Vector3 noY = lastFrameVel;
        float y = noY.y; noY.y = 0;
        noY += (dir * Time.deltaTime);
        noY = Vector3.ClampMagnitude(noY, airMoveMaxSpd);
       lastFrameVel = noY;
        lastFrameVel.y = y;
    }

    public void ApplyAirDrag()
    {
       
        Vector3 noY = lastFrameVel;
        float y = noY.y;
        noY.y = 0;
        float mag = noY.magnitude;
        mag -= (airDragStr * Time.deltaTime);
        noY = Vector3.ClampMagnitude(noY, mag);
        lastFrameVel = noY;
        lastFrameVel.y = y;
    }

    public bool CutOffJump(float strength, float minCutOff, float maxCutOff)
    {
        if (lastFrameVel.y < maxCutOff && lastFrameVel.y > minCutOff) { lastFrameVel.y *= strength; return true; } return false;
    }


    public void OnGroundLeave()
    {

    }

    public void OnGroundEnter()
    {
        Vector3 airVel = lastFrameVel;
        airVel.y = 0;
        moveSpd = airVel.magnitude;
        finalDir = airVel.normalized;
        
    }

    public bool IsFalling()
    {
        return lastFrameVel.y < 0;
    }

    public void ApplyJump(Vector3 direction, float strength, float airMoveAmount, float velModifier, bool add, float airDragAmount, float gravStr)
    {
        JumpBuffer = false;
        if (add)
        {
            lastFrameVel += strength * direction;
        }
        else
        {
            lastFrameVel += (strength * direction);
            lastFrameVel.y = strength;
        }

        airMoveSpd = airMoveAmount;
        airDragStr = airDragAmount;
        airMoveMaxSpd = speedTarget * velModifier;
        gravityStr = gravStr;
    }


    private void LateUpdate()
    {
        CollisionDetection();
    }
    void DrawDebug()
    {
        SetColliderOffets();
        Debug.DrawLine(p1, p2, Color.blue);
        Debug.DrawLine(transform.position + new Vector3(-col.radius, 0, 0), transform.position + new Vector3(col.radius, 0, 0), Color.blue);
        Debug.DrawLine(transform.position + new Vector3(0, 0, -col.radius), transform.position + new Vector3(0, 0, col.radius), Color.blue);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(DEBUG_colOverlapPoint, 0.2f);

    }

  
    public IEnumerator UnstickGroundTimer()
    {
        isGrounded = false;
        GroundUnstick = true;
        yield return new WaitForSeconds(0.1f);
        GroundUnstick = false;
    }






}
