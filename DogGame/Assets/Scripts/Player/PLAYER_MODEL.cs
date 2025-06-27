using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_MODEL : MonoBehaviour
{
    PLAYER_MOVEMENT PM;
    GameObject Player;
    public float lerpSpd;
    public float turnSpd;
    public float slopeAlignspd;
    Vector3 rot;
    Vector3 slopeAlign;
    public bool alignToSlope;
    float roty;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        PM = GetComponentInParent<PLAYER_MOVEMENT>();
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Player.transform.localScale;
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (PM.isGrounded)
        {
            float y = Mathf.Lerp(transform.position.y, Player.transform.position.y, lerpSpd * Time.deltaTime);
            transform.position = new Vector3(Player.transform.position.x, y, Player.transform.position.z);
        }
        else { transform.position = Player.transform.position;}

        Vector3 rotTarget = PM.targetDir;
        if (!alignToSlope) { rotTarget.y = 0; rotTarget.Normalize(); }
        if (PM.cutscene) return;

        roty = Mathf.Lerp(roty, rotTarget.y, slopeAlignspd * Time.deltaTime);
        rot = Vector3.Lerp(rot, rotTarget, turnSpd * Time.deltaTime);
        rot.y = roty;
        if (rot != Vector3.zero) 
        {
            // transform.rotation = Quaternion.LookRotation(rot);
            Vector3 dir = PM.finalDir;
            dir.y = 0;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);

            }
        }

        slopeAlign = Vector3.Lerp(slopeAlign, PM.WinningGroundCast.normal, slopeAlignspd * Time.deltaTime) ;
        Vector3 desiredSlopeRot = Vector3.Cross(transform.right, slopeAlign);
        if (desiredSlopeRot != Vector3.zero) 
        {
            transform.rotation = Quaternion.LookRotation(desiredSlopeRot);

        }
    }
}
