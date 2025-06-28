using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Assertions;

public class NPCModel : MonoBehaviour, I_Interactable
{
    [SerializeField] private NPCBone.NPCBodyType _bodyType;
    [SerializeField] private List<NPCModelAttachment> _attachments = new();
    [SerializeField] private bool _hasAnimation = false;
    [SerializeField] private NPCAnimation _animation;

    private readonly Dictionary<NPCModelAttachment, GameObject> _attachmentToGameObject = new();
    private NPCAnimationContainer _animations;
    private AnimationClip _animationToPlay;
    private PlayableGraph _graph;
    private AnimationClipPlayable _mainPlayable;
    private AnimationClipPlayable _barkPlayable;
    private PlayableOutput _playableOutput;
    private bool _playingBark = false;
    private AnimationMixerPlayable _mixer;
    private double _barkEndTime = 0;
    private const float CROSSFADE_DURATION = 0.2f;

    private void Awake()
    {
        _animations = FindAnyObjectByType<NPCAnimationContainer>();
        Assert.IsNotNull(_animations);

        if (_hasAnimation)
        {
            Assert.IsTrue(_animations.CanPlay(_bodyType, _animation));
            _animationToPlay = _animations.GetAnimation(_animation);
            Assert.IsNotNull(_animationToPlay);

            GameObject rootBone = _animations.GetRootBone(gameObject, _bodyType);
            Assert.IsNotNull(rootBone);

            var animator = rootBone.AddComponent<Animator>();

            _graph = PlayableGraph.Create($"NPCModelGraph_{name}");
            _playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", animator);

            _mainPlayable = AnimationClipPlayable.Create(_graph, _animationToPlay);
            _mainPlayable.SetApplyFootIK(false);
            _mainPlayable.SetTime(0);
            _mainPlayable.SetDuration(double.PositiveInfinity);
            _animationToPlay.wrapMode = WrapMode.Loop;

            _playableOutput.SetSourcePlayable(_mainPlayable);
            _graph.Play();
        }

        Assert.IsNotNull(_attachments);
        foreach (var attachment in _attachments)
        {
            _attachmentToGameObject.Add(attachment, attachment.Attach(gameObject, _bodyType));
        }
    }

    private void Update()
    {
        if (_hasAnimation && !_graph.IsPlaying())
        {
            _graph.Play();
        }

        if (_playingBark && _barkPlayable.IsValid() && _barkPlayable.GetTime() >= _barkPlayable.GetDuration())
        {
            _mainPlayable.SetTime(0);
            _playableOutput.SetSourcePlayable(_mainPlayable);
            _playingBark = false;
        }
    }

    private void OnDestroy()
    {
        if (_hasAnimation && _graph.IsValid()) _graph.Destroy();
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
        if (!_hasAnimation || _playingBark) return;

        AnimationClip barkClip = _animations.GetBarkReaction(_bodyType);
        if (barkClip == null) return;

        _barkPlayable = AnimationClipPlayable.Create(_graph, barkClip);
        _barkPlayable.SetApplyFootIK(false);
        _barkPlayable.SetTime(0);
        _barkPlayable.SetDuration(barkClip.length);
        barkClip.wrapMode = WrapMode.Once;
        

        _playableOutput.SetSourcePlayable(_barkPlayable);
        _playingBark = true;
    }
}
