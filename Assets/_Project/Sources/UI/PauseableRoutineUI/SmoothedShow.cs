using Pause;
using UnityEngine;

namespace UI.PauseableRoutineUI
{
    public class SmoothedShow : PauseableRoutine
    {
        [SerializeField] private CanvasGroup[] _canvasGroups;

        private float StartValue;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            StartValue = 0f;
            TargetValue = 1f;

            foreach (CanvasGroup group in _canvasGroups)
                group.alpha = StartValue;
        }

        private protected override void OnRoutineStart()
        {
            StartValue = _canvasGroups[0].alpha;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;

            foreach (CanvasGroup group in _canvasGroups)
                group.alpha = Mathf.Lerp(StartValue, TargetValue, progress);
        }
    }
}