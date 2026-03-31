using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class SmoothedFade : PauseableRoutine
    {
        [SerializeField] private protected CanvasGroup CanvasGroup;
        [SerializeField] private protected List<GameObject> Elements;

        private protected float StartValue;

        private protected override void OnDisable()
        {
            CanvasGroup.alpha = 0;
            base.OnDisable();
        }

        public void FadeIn(float duration, float alpha)
        {
            alpha = Mathf.Clamp01(alpha);

            if(Mathf.Approximately(CanvasGroup.alpha, alpha) == false)
                UpdateView(duration, alpha);
        }

        public void FadeOut(float duration)
        {
            if (Mathf.Approximately(CanvasGroup.alpha, 0) == false)
            {
                TargetValue = 0;
                UpdateView(duration);
            }  
        }

        private protected override void OnRoutineStart()
        {
            UserUtils.SetActiveElements(true, Elements);
            StartValue = CanvasGroup.alpha;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;
            CanvasGroup.alpha = Mathf.Lerp(StartValue, TargetValue, progress);
        }

        private protected override void OnRoutineEnd()
        {
            CanvasGroup.alpha = TargetValue;

            if (Mathf.Approximately(TargetValue, 0))
                UserUtils.SetActiveElements(false, Elements);

            base.OnRoutineEnd();
        }
    }
}