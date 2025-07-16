using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class SmoothedFade : PauseableRoutine
    {
        private const float DefaultAlpha = 1.0f;

        [SerializeField] private protected CanvasGroup CanvasGroup;
        [SerializeField] private protected List<GameObject> Elements;

        private protected float StartAlpha;
        private protected float StartValue;
 
        public void ShowElements()
        {
            if (Routine != null)
                StopCoroutine(Routine);

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
            if (Routine != null)
                StopCoroutine(Routine);

            CanvasGroup.alpha = 0;
            UserUtils.SetActiveElements(false, Elements);
        }

        public void FadeOut()
        {
            UpdateView(Duration);
        }

        private protected override void OnRoutineStart() 
        {
            StartValue = CanvasGroup.alpha;
        }

        private protected override void OnRoutineIteration(float cycleDuration) 
        {
            float progress = ElapsedTime / cycleDuration;
            CanvasGroup.alpha = Mathf.Lerp(StartValue, 0f, progress);
        }

        private protected override void OnRoutineEnd()
        {
            base.OnRoutineEnd();
            CanvasGroup.alpha = 0;
            UserUtils.SetActiveElements(false, Elements);
        }
    }
}