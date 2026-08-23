using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class AnimationSpawner
    {
        private const float BaseAnimationLength = 2;

        public static Sequence GetMoveScaleAnimation(
            RectTransform transform, Vector2 offset, float scaleMul = 0.75f, float duration = 0.75f)
        {
            Vector2 startPos = transform.anchoredPosition;
            Vector3 startScale = transform.localScale;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOAnchorPos(startPos + offset, duration))
                    .Insert(0f, transform.DOScale(startScale * scaleMul, duration))
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear)
                    .SetAutoKill(false)
                    .SetLink(transform.gameObject)
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
            Vector2 endPos = startPos + (Vector2.up * offsetY);

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

        public static Sequence GetFadeAnimation(
            CanvasGroup canvasGroup, float startValue, float endValue, float duration = 0)
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

        public static Sequence GetMenuWindowAnimation(
            RectTransform transform, CanvasGroup canvasGroup, float startValue, float endValue, float duration = 0)
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

        public static Sequence GetTrapGrowUpAnimation(Transform transform, float defaultSize)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(defaultSize, UserUtils.GrowDuration).From(0))
                    .SetAutoKill(false)
                    .SetEase(Ease.Linear)
                    .Pause();

            return sequence;
        }

        public static Sequence GetTrapGrowDownAnimation(Transform transform, float defaultSize)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0, UserUtils.GrowDuration).From(defaultSize))
                    .SetAutoKill(false)
                    .SetEase(Ease.Linear)
                    .Pause();

            return sequence;
        }

        public static Sequence GetOptionsShowAnimation(RectTransform transform, CanvasGroup canvasGroup, float duration)
        {
            float startScaleX = transform.localScale.x;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(GetFadeAnimation(canvasGroup, 0, 1, duration * 0.25f));

            sequence.Insert(0, transform.DOScale(startScaleX * 1.1f, duration * 0.55f)
                    .From(0)
                    .SetEase(Ease.OutBack));

            sequence.Insert(0, transform.DORotate(new Vector3(0, 0, 360), duration * 0.55f, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutCubic));

            sequence.Append(transform.DOScale(startScaleX, duration * 0.2f)
                    .SetEase(Ease.OutSine));

            sequence.Append(transform.DOShakeRotation(duration * 0.25f, strength: 10f, vibrato: 15, randomness: 45));

            sequence.SetAutoKill(false);
            sequence.Pause();

            return sequence;
        }

        public static Sequence GetStatAnimation(RectTransform transform, float duration)
        {
            float transformScaleX = transform.localScale.x;

            Sequence sequence = DOTween.Sequence();

            sequence = GetShakeAnimation(transform, duration);

            sequence.Insert(0, transform.DOScale(transformScaleX * 1.2f, duration / 2)
                    .From(transformScaleX)
                    .SetEase(Ease.OutBack));

            sequence.Insert(1, transform.DOScale(transformScaleX, duration / 2))
                    .SetEase(Ease.OutSine);

            sequence.SetAutoKill(false);
            sequence.Pause();

            return sequence;
        }

        public static Sequence GetOptionsHideAnimation(RectTransform transform, CanvasGroup canvasGroup, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DORotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutCubic));

            sequence.Insert(0, transform.DOScale(0, duration)
                    .From(1)
                    .SetEase(Ease.OutBack));

            sequence.Insert(0, GetFadeAnimation(canvasGroup, 1, 0, duration * 0.5f));

            sequence.SetAutoKill(false);
            sequence.Pause();

            return sequence;
        }
    }
}