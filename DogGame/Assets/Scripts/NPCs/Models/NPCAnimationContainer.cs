using UnityEngine;
using UnityEngine.Assertions;

public enum NPCAnimation
{
    HulaDance, SandAngel, Sightseeing, SittingAndDrinking, DrinkingBeerAndScratchingAss, MuscleFlex
}

public class NPCAnimationContainer : MonoBehaviour
{
    [SerializeField] private NPCBone _rootBone;

    [Header("Bark reactions")]
    [SerializeField] private AnimationClip _shirtHatBarkReaction;
    [SerializeField] private AnimationClip _femaleMaleSunglassesBarkReaction;

    [Header("Sunglasses, female, male")]
    [SerializeField] private AnimationClip _hulaDance;
    [SerializeField] private AnimationClip _sandAngel;
    [SerializeField] private AnimationClip _sightseeing;
    [SerializeField] private AnimationClip _sittingAndDrinking;

    [Header("Shirt, hat")]
    [SerializeField] private AnimationClip _drinkingBeerAndScratchingAss;
    [SerializeField] private AnimationClip _muscleFlex;

    public GameObject GetRootBone(GameObject model, NPCBone.NPCBodyType bodyType)
    {
        return NPCModelAttachment.FindGameObjectInChildrenByName(model, _rootBone.GetBoneName(bodyType));
    }

    public AnimationClip GetAnimation(NPCAnimation anim)
    {
        switch (anim)
        {
            case NPCAnimation.HulaDance: return _hulaDance;
            case NPCAnimation.SandAngel: return _sandAngel;
            case NPCAnimation.Sightseeing: return _sightseeing;
            case NPCAnimation.SittingAndDrinking: return _sittingAndDrinking;
            case NPCAnimation.DrinkingBeerAndScratchingAss: return _drinkingBeerAndScratchingAss;
            case NPCAnimation.MuscleFlex: return _muscleFlex;
            default:
                Assert.IsTrue(false, $"invalid animation {GetComponent<Animation>()} ");
                return null;
        }
    }

    public bool CanPlay(NPCBone.NPCBodyType type, NPCAnimation animation)
    {
        switch (animation)
        {
            case NPCAnimation.HulaDance:
            case NPCAnimation.SandAngel:
            case NPCAnimation.Sightseeing:
            case NPCAnimation.SittingAndDrinking:
                return type == NPCBone.NPCBodyType.Male || type == NPCBone.NPCBodyType.Female || type == NPCBone.NPCBodyType.SunglassesLady;
            case NPCAnimation.DrinkingBeerAndScratchingAss:
            case NPCAnimation.MuscleFlex:
                return type == NPCBone.NPCBodyType.ShirtGuy || type == NPCBone.NPCBodyType.HatGuy;
            default:
                Assert.IsTrue(false, $"invalid animation {animation} or type {type}");
                return false;
        }
    }

    public AnimationClip GetBarkReaction(NPCBone.NPCBodyType type)
    {
        if (type == NPCBone.NPCBodyType.Male || type == NPCBone.NPCBodyType.Female || type == NPCBone.NPCBodyType.SunglassesLady)
        {
            return _femaleMaleSunglassesBarkReaction;
        }
        else if (type == NPCBone.NPCBodyType.ShirtGuy || type == NPCBone.NPCBodyType.HatGuy)
        {
            return _shirtHatBarkReaction;
        }
        else
        {
            Assert.IsTrue(false, $"failed to get bark reaction animation, invalid type {type}");
            return null;
        }
    }
}
