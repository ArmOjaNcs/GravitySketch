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

        public static Sequence GetArrowAnimation(RectTransform transform)
        {
            Vector2 startAnchoredPos = transform.anchoredPosition;
            Vector3 startScale = transform.localScale;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOAnchorPosX(startAnchoredPos.x - 50f, 0.75f))
                    .Insert(0, transform.DOScale(startScale * 0.75f, 0.75f))
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(transform.gameObject)
                    .SetEase(Ease.Linear)
                    .SetAutoKill(false)
                    .Pause();

            return sequence;
        }

        public static Sequence GetAimAnimation(RectTransform transform)
        {
            Vector2 startAnchoredPos = transform.anchoredPosition;
            Vector3 startScale = transform.localScale;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(startScale * 0.75f, 0.75f))
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(transform.gameObject)
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

        public static Sequence GetPopUpAnimation(RectTransform rectTransform, float offsetY, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            Vector2 startPos = rectTransform.anchoredPosition;
            Vector2 endPos = startPos + Vector2.up * offsetY;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOAnchorPos(endPos, duration).From(startPos))
                    .SetEase(Ease.OutQuad)
                    .SetAutoKill(false)
                    .Pause();

            return sequence;
        }

        public static Sequence GetShakeAnimation(RectTransform transform, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOShakeAnchorPos(duration, 10).SetEase(Ease.Linear))
                    .SetAutoKill(false)
                    .Pause();

            return sequence;
        }

        public static Sequence GetFadeAnimation(CanvasGroup canvasGroup, float startValue, 
            float endValue, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(endValue, duration).From(startValue))
                    .SetAutoKill(false)
                    .SetEase(Ease.Linear)
                    .Pause();

            return sequence;
        }

        public static Sequence GetMenuWindowAnimation(RectTransform transform, CanvasGroup canvasGroup, 
            float startValue, float endValue, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            Sequence sequence = GetFadeAnimation(canvasGroup, startValue, endValue, duration);
            sequence.Insert(0, transform.DOScale(endValue, duration).From(startValue))
                    .SetAutoKill(false)
                    .SetEase(Ease.Linear)
                    .Pause();

            return sequence;
        }

        public static Tween GetLoadAnimation(RectTransform transform, float duration = 0)
        {
            if (duration <= 0)
                duration = BaseAnimationLength;

            return transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360)
                .SetAutoKill(false)
                .SetLoops(-1)
                .SetEase(Ease.Linear)
                .SetLink(transform.gameObject);
        }

        public static Sequence GetOptionsShowAnimation(RectTransform transform, float duration)
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

        public static Sequence GetOptionsHideAnimation(RectTransform transform, float duration)
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