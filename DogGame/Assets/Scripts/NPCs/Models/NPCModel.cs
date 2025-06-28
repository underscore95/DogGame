using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Assertions;
using System.Collections;

public class NPCModel : MonoBehaviour, I_Interactable
{
    [SerializeField] private NPCBone.NPCBodyType _bodyType;
    [SerializeField] private List<NPCModelAttachment> _attachments = new();

    [Header("Animation")]
    [SerializeField] private NPCAnimation _animation;
    [SerializeField] private bool _reactsToBarks = true;
    [SerializeField] private float _pauseDurationAfterBark = 0.8f;

    private readonly Dictionary<NPCModelAttachment, GameObject> _attachmentToGameObject = new();
    private NPCAnimationContainer _animations;
    private Animator _animator;
    private float _barkDuration = 0;

    private void Awake()
    {
        // attachments
        Assert.IsNotNull(_attachments);
        foreach (var attachment in _attachments)
        {
            _attachmentToGameObject.Add(attachment, attachment.Attach(gameObject, _bodyType));
        }

        // animation
        SetupAnimation();
    }

    private void SetupAnimation()
    {
        // get root bone, animation clips
        _animations = FindAnyObjectByType<NPCAnimationContainer>();
        Assert.IsNotNull(_animations);

        Assert.IsTrue(_animations.CanPlay(_bodyType, _animation), $"Can't play animation {_animation} on body type {_bodyType} ({gameObject.name})");
        AnimationClip animationToPlay = _animations.GetAnimation(_animation);
        Assert.IsNotNull(animationToPlay);

        GameObject rootBone = _animations.GetRootBone(gameObject, _bodyType);
        Assert.IsNotNull(rootBone);

       //Assert.IsTrue(animationToPlay.wrapMode == WrapMode.Loop);

        AnimationClip barkClip = _animations.GetBarkReaction(_bodyType);
        Assert.IsNotNull(barkClip);
      //  Assert.IsTrue(barkClip.wrapMode == WrapMode.Once);
        _barkDuration = barkClip.length;

        // setup animator
        _animator = rootBone.AddComponent<Animator>();
        var controller = new AnimatorOverrideController
        {
            runtimeAnimatorController = _animations.GetNPCBaseController()
        };
        Assert.IsNotNull(controller);
        Assert.IsNotNull(controller.runtimeAnimatorController);
        Assert.IsNotNull(controller.runtimeAnimatorController.animationClips);

        // animator has dummy clips in it, replace them with the correct ones
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var clip in controller.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "ShirtHatBarkReaction") overrides.Add(new(clip, animationToPlay));
            else if (clip.name == "FemaleMaleBarkReaction") overrides.Add(new(clip, barkClip));
        }
        controller.ApplyOverrides(overrides);

        _animator.runtimeAnimatorController = controller;
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {

    }

    public bool HasAttachment(NPCModelAttachment attachment)
    {
        return _attachmentToGameObject.ContainsKey(attachment);
    }

    public void RemoveAttachment(NPCModelAttachment attachment)
    {
        Assert.IsTrue(HasAttachment(attachment));
        Destroy(_attachmentToGameObject[attachment]);
        _attachmentToGameObject.Remove(attachment);
    }

    public void OnEnterInteractRange() { }
    public void OnExitInteractRange() { }
    public void InteractableInRange() { }

    public void InteractableAction()
    {
        if (_reactsToBarks)
        {
            _animator.SetTrigger("TriggerBark");
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(_barkDuration + _pauseDurationAfterBark);
        _animator.SetTrigger("TriggerIdle");

    }
}