using UnityEngine;
using UnityEngine.Assertions;

public class InterpolatedTransform : MonoBehaviour
{
    public struct LocalTransform
    {
        public Vector3 Position;
        public Vector3 Scale;
        public Quaternion Rotation;

        public LocalTransform(Transform t)
        {
            Position = t.localPosition;
            Scale = t.localScale;
            Rotation = t.localRotation;
        }

        public override readonly string ToString()
        {
            return $"LocalTransform{{   Position: {Position}   Rotation: {Rotation.eulerAngles}   Scale: {Scale}   }}";
        }
    }

    private LocalTransform _start;
    private LocalTransform _end;
    private float _duration = 0;
    private float _elapsed;

    private void Update()
    {
        _elapsed += Time.deltaTime;
        float t = _elapsed / _duration; // 0 to ~1

        transform.SetLocalPositionAndRotation(Vector3.Lerp(_start.Position, _end.Position, t), Quaternion.Lerp(_start.Rotation, _end.Rotation, t));
        transform.localScale = Vector3.Lerp(_start.Scale, _end.Scale, t);

        if (t >= 1.0f)
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Linearly move an object to a new transform
    /// </summary>
    /// <param name="obj">Object</param>
    /// <param name="end">Destination</param>
    /// <param name="duration">Seconds to interplate for</param>
    public static void StartInterpolation(GameObject obj, LocalTransform end, float duration = 0.5f)
    {
        if (!obj.TryGetComponent<InterpolatedTransform>(out var i))
        {
            i = obj.AddComponent<InterpolatedTransform>();
        }

        i._elapsed = 0;
        i._duration = duration;
        i._start = new(obj.transform);
        i._end = end;
    }
}
