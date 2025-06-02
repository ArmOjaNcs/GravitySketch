using DG.Tweening;
using UnityEngine;

public static class AnimationSpawner
{
    private const float BaseAnimationLength = 2;

    private static readonly Vector3[] _rotations =
    {
        new Vector3(UserUtils.MaxRotation, 0, 0),
        new Vector3(0, UserUtils.MaxRotation, 0),
        new Vector3(0, 0, UserUtils.MaxRotation)
    };

    public static Sequence GetIdleAnimation(Transform transform)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DORotate(GetRandomRotation(), BaseAnimationLength, RotateMode.FastBeyond360))
            .Join(transform.DOMoveY(startPosition.y + 0.5f, BaseAnimationLength / UserUtils.Half))
            .Insert(1, transform.DOMoveY(startPosition.y, BaseAnimationLength / UserUtils.Half))
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .SetAutoKill(false);

        return sequence;
    }

    public static Tween GetDissolveAnimation(Transform transform, float duration = 0)
    {
        if(duration > 0) 
            return transform.DOScale(0, duration).SetLink(transform.gameObject);

        return transform.DOScale(0, BaseAnimationLength).SetLink(transform.gameObject);
    }

    public static Sequence GetCatchedAnimation(Transform transform, Transform hole)
    {
        Sequence sequence = DOTween.Sequence();
        Vector3 holePosition = hole.position;
        holePosition.y = transform.position.y;
        sequence.Append(transform.DOMove(holePosition, 1))
            .Join(transform.DORotate(GetRandomRotation(), 1, RotateMode.FastBeyond360))
            .SetEase(Ease.Linear)
            .SetAutoKill(false);

        return sequence;
    }

    private static Vector3 GetRandomRotation()
    {
        int rotationsIndex = Random.Range(0, _rotations.Length);

        return _rotations[rotationsIndex];
    }
}