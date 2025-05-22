using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SimpleCubeAnimationSpawner 
{
    public Sequence GetIdleAnimation(Transform transform)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DORotate(GetRandomRotation(), 2f, RotateMode.FastBeyond360))
            .Join(transform.DOMoveY(startPosition.y + 0.5f, 1f))
            .Insert(1, transform.DOMoveY(startPosition.y, 1f))
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .SetAutoKill(false);

        return sequence;
    }

    public Sequence GetDissolveAnimation(Transform cube, Vector3 destination)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(cube.DOMove(destination, 1.5f))
            .Join(cube.DOScale(0, 1.5f))
            .SetAutoKill(false);

        return sequence;
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