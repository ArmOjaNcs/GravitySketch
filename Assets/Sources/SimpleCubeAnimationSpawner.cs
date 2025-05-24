using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SimpleCubeAnimationSpawner
{
    private const float BaseAnimationLength = 2;

    public Sequence GetIdleAnimation(Transform transform)
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

    public Tween GetDissolveAnimation(Transform cube)
    {
        return cube.DOScale(0, BaseAnimationLength).SetLink(cube.gameObject);
    }

    private Vector3 GetRandomRotation()
    {
        int rotationsIndex = Random.Range(0, _rotations.Length);

        return _rotations[rotationsIndex];
    }

    private readonly Vector3[] _rotations =
    {
        new Vector3(UserUtils.MaxRotation, 0, 0),
        new Vector3(0, UserUtils.MaxRotation, 0),
        new Vector3(0, 0, UserUtils.MaxRotation)
    };
}