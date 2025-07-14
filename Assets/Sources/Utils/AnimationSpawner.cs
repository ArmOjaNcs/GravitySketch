using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.Utils
{
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
            if (duration <= 0)
                duration = BaseAnimationLength;

            return transform.DOScale(0, duration).SetLink(transform.gameObject);
        }

        public static Tween GetPopUpAnimation(Transform transform, float offsetZ, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            Vector3 startPos = transform.localPosition;
            Vector3 endPos = startPos + Vector3.forward * offsetZ;

            return transform.DOLocalMove(endPos, duration)
                .SetEase(Ease.OutQuad)
                .From(startPos)
                .SetAutoKill(false)
                .Pause();
        }

        public static Tween GetPopUpAnimation(RectTransform rectTransform, float offsetY, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            Vector2 startPos = rectTransform.anchoredPosition;
            Vector2 endPos = startPos + Vector2.up * offsetY;

            return rectTransform.DOAnchorPos(endPos, duration)
                .SetEase(Ease.OutQuad)
                .From(startPos)
                .SetAutoKill(false)
                .Pause();
        }

        public static Tween GetShakeAnimation(Transform transform, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            return transform.DOShakePosition(duration, 10).SetEase(Ease.Linear).SetAutoKill(false);
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
}