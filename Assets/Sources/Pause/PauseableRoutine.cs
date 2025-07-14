using System.Collections;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public abstract class PauseableRoutine : PauseableObject
    {
        private const float MinDuration = 0.5f;

        [SerializeField, Min(0)] private protected float Duration;

        private protected Coroutine Routine;
        private protected float CurrentTime;
        private protected float TargetValue;

        private protected virtual void OnDisable()
        {
            if (Routine != null)
                StopCoroutine(Routine);

            Routine = null;
        }

        private protected virtual void Start()
        {
            if (Mathf.Approximately(Duration, 0))
                Duration = MinDuration;
        }

        public override void Pause()
        {
            base.Pause();

            if (Routine != null && isActiveAndEnabled)
                StopCoroutine(Routine);
        }

        public override void Resume()
        {
            base.Resume();

            if (Routine != null && CurrentTime < Duration && isActiveAndEnabled)
                Routine = StartCoroutine(UpdateRoutine(Duration - CurrentTime));
        }

        public void UpdateView(float duration, float target)
        {
            if (isActiveAndEnabled == false)
                return;

            TargetValue = target;

            if (Routine != null)
                StopCoroutine(Routine);

            Routine = StartCoroutine(UpdateRoutine(duration));
        }

        private protected abstract IEnumerator UpdateRoutine(float duration);

        private protected virtual void OnUpdate()
        {
            if (isActiveAndEnabled == false)
                return;

            if (Routine != null)
                StopCoroutine(Routine);

            Routine = StartCoroutine(UpdateRoutine(Duration));
        }
    }
}