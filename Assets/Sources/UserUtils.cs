using DG.Tweening;
using UnityEngine;

public static class UserUtils 
{
    public const int ImageResolution = 32;
    public const int ColorizerBarCount = 5;
    public const int MinRotateSpeed = 10;
    public const int MaxRotateSpeed = 50;
    public const int MaxRotation = 360;

    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);

    public const float Half = 2;
    public const float HalfUnit = 0.5f;
    public const float TimeForShow = 5f;

    private const float MinAlfa = 0.1f;

    private static readonly Vector3[] _rotations =
    {
        new Vector3(MaxRotation, 0, 0),
        new Vector3(0, MaxRotation, 0), 
        new Vector3(0, 0, MaxRotation)
    };

    public static bool IsBlack(Color color) 
    {
        return Mathf.Approximately(color.r, 0)
            && Mathf.Approximately(color.g, 0)
            && Mathf.Approximately(color.b, 0);
    }

    public static bool IsTransparent(Color color) => color.a < MinAlfa;

    public static Vector3 GetRandomRotateDirection()
    {
        float xRotation = Random.Range(0, MaxRotation);
        float yRotation = Random.Range(0, MaxRotation);
        float zRotation = Random.Range(0, MaxRotation);

        return new Vector3(xRotation, yRotation, zRotation);
    }

    public static Sequence GetAnimation(Transform transform)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        int rotationsIndex = Random.Range(0, _rotations.Length);

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DORotate(_rotations[rotationsIndex], 2f, RotateMode.FastBeyond360))
            .Join(transform.DOMoveY(startPosition.y + 0.5f, 1f))
            .Insert(1, transform.DOMoveY(startPosition.y, 1f))
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .SetLink(transform.gameObject);

        return sequence;
    }
}