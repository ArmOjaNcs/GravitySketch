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
                .Join(transform.DOMoveY(startPosition.y + 0.5f, BaseAnimationLength / UserUtils.Two))
                .Insert(1, transform.DOMoveY(startPosition.y, BaseAnimationLength / UserUtils.Two))
                .SetLoops(-1)
                .SetEase(Ease.Linear)
                .SetAutoKill(false)
                .Pause();

            return sequence;
        }

        public static Tween GetDissolveAnimation(Transform transform, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            return transform.DOScale(0, duration).SetLink(transform.gameObject).Pause();
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

        public static Tween GetShakeAnimation(RectTransform transform, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            return transform.DOShakeAnchorPos(duration, 10).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        }

        public static Sequence GetShowAnimation(RectTransform transform, float duration)
        {
            float transformScaleX = transform.localScale.x;

            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(transform.DOScale(transformScaleX * 1.1f, duration * 0.5f)
                    .From(0)
                    .SetEase(Ease.OutBack));

            sequence.Append(transform.DOScale(transformScaleX, duration * 0.2f)
                    .SetEase(Ease.OutSine));

            sequence.Append(GetShakeAnimation(transform, duration * 0.3f));
            sequence.SetAutoKill(false);
            sequence.Pause();

            return sequence;
        }

        public static Sequence GetHideAnimation(RectTransform transform, float duration)
        {
            Vector3 originalScale = Vector3.one;
            float step = duration / 3;
           
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOScaleX(originalScale.x * 0.5f, step)
                    .SetEase(Ease.Linear));

            sequence.Append(transform.DOScaleY(originalScale.y * 0.5f, step)
                    .SetEase(Ease.Linear));

            sequence.Append(transform.DOScale(Vector3.zero, step)
                    .SetEase(Ease.InBack));

            sequence.SetAutoKill(false);
            sequence.Pause();

            return sequence;
        }

        private static Vector3 GetRandomRotation()
        {
            int rotationsIndex = Random.Range(0, _rotations.Length);

            return _rotations[rotationsIndex];
        }
    }
}