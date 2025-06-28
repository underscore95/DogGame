using NUnit.Framework.Constraints;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class PLAYER_ANIMATION : MonoBehaviour
{
    [SerializeField] Animator ANIM;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Bark = Animator.StringToHash("Bark");
    private static readonly int Sunscreen = Animator.StringToHash("Sunscreen");

    [SerializeField] Animator AnimDogShirt;

    int storedAnim;
    PLAYER_STATES PS;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PS = GetComponent<PLAYER_STATES>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (PS.StateGrp)
        {
            case PLAYER_STATES.StateGroup.GroundStates:
                SetAnimation(GroundedAnims(), 0.4f);
                break;
            case PLAYER_STATES.StateGroup.AirStates:               
                SetAnimation(AirBorneAnims(), 0.4f);
                break;

        }
    }

    public void PlayBarkAnim()
    {
        ANIM.Play(Bark, 1);
        StartCoroutine(StopBark());
    }

    IEnumerator StopBark()
    {
        yield return new WaitForSeconds(0.3f);
        ANIM.Play(Idle,1);
    }

   int GroundedAnims()
        {
        switch (PS.GState)
        {
            case PLAYER_STATES.GrndStates.Idle:
                return Idle;
            case PLAYER_STATES.GrndStates.Walking:
                return Walk;
            case PLAYER_STATES.GrndStates.Running:
                return Run ;
            case PLAYER_STATES.GrndStates.Suncream:
                return Sunscreen;
           
        }
        return Idle;
        }

    int  AirBorneAnims()
        {
        switch (PS.AState)
        {
            case PLAYER_STATES.AirStates.Jumping:
                return Walk;
                 
            case PLAYER_STATES.AirStates.Falling:
                return Walk;
                
        }
        return Walk;
        }

    void SetAnimation(int anim, float crossfadeTime)
    {
        if (anim == storedAnim) { }
        else
        {
            storedAnim = anim;
            ANIM.CrossFadeInFixedTime(anim, 0.1f);
            AnimDogShirt.CrossFadeInFixedTime(anim, 0.1f);
        }
    }

 


}
