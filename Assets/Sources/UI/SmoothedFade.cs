using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Pause;

namespace Assets.Sources.UI
{
    public class SmoothedFade : PauseableObject
    {
        private const float DefaultAlpha = 1.0f;

        [SerializeField] private protected float FadeDuration;
        [SerializeField] private protected CanvasGroup CanvasGroup;
        [SerializeField] private protected List<GameObject> Elements;

        private protected Coroutine FadeRoutine;
        private protected float CurrentTime;
        private protected float StartAlpha;

        public void ShowElements()
        {
            if (FadeRoutine != null)
                StopCoroutine(FadeRoutine);

            if(Mathf.Approximately(StartAlpha, 0))
                StartAlpha = DefaultAlpha;

            CanvasGroup.alpha = StartAlpha;
            UserUtils.SetActiveElements(true, Elements);
        }

        public void SetStartAplpha(float alpha)
        {
            alpha = Mathf.Clamp01(alpha);
            StartAlpha = alpha;
        }

        public void HideElements()
        {
            if (FadeRoutine != null)
                StopCoroutine(FadeRoutine);

            CanvasGroup.alpha = 0;
            UserUtils.SetActiveElements(false, Elements);
        }

        public override void Pause()
        {
            base.Pause();

            if (FadeRoutine != null)
                StopCoroutine(FadeRoutine);
        }

        public override void Resume()
        {
            base.Resume();

            if (FadeRoutine != null && CurrentTime < FadeDuration && isActiveAndEnabled)
                FadeRoutine = StartCoroutine(FadeOut(FadeDuration - CurrentTime, CanvasGroup, Elements));
        }

        public void FadeOut()
        {
            if (isActiveAndEnabled == false)
                return;

            if (FadeRoutine != null)
                StopCoroutine(FadeRoutine);

            FadeRoutine = StartCoroutine(FadeOut(FadeDuration, CanvasGroup, Elements));
        }

        private protected IEnumerator FadeOut(float duration, CanvasGroup canvasGroup,
            List<GameObject> gameObjects = null)
        {
            float startValue = canvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                if (CurrentTime < elapsedTime)
                    CurrentTime = elapsedTime;

                canvasGroup.alpha = Mathf.Lerp(startValue, 0f, elapsedTime / duration);
                yield return null;
            }

            canvasGroup.alpha = 0;
            UserUtils.SetActiveElements(false, gameObjects);
            FadeRoutine = null;
        }
    }
}