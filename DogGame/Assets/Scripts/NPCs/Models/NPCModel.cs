using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Assertions;

public class NPCModel : MonoBehaviour
{
    [SerializeField] private NPCBone.NPCBodyType _bodyType;
    [SerializeField] private List<NPCModelAttachment> _attachments = new();
    [SerializeField] private bool _hasAnimation = false;
    [SerializeField] private NPCAnimation _animation;

    private readonly Dictionary<NPCModelAttachment, GameObject> _attachmentToGameObject = new();
    private NPCAnimationContainer _animations;
    private AnimationClip _animationToPlay;
    private PlayableGraph _graph;

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

            var animator = rootBone.GetComponent<Animator>();
            if (animator == null)
                animator = rootBone.AddComponent<Animator>();

            _graph = PlayableGraph.Create($"NPCModelGraph_{name}");
            var playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", animator);

            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(_graph, _animationToPlay);
            clipPlayable.SetApplyFootIK(false);
            clipPlayable.SetTime(0);
            _animationToPlay.wrapMode = WrapMode.Loop;

            playableOutput.SetSourcePlayable(clipPlayable);
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
}
