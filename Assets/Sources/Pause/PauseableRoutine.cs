using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class PauseableRoutine : PauseableObject
    {
        private const float MinDuration = 0.5f;

        [SerializeField, Min(0)] private protected float Duration;

        private protected Coroutine Routine;
        private protected float TargetValue;
        private protected float ElapsedTime;

        public event Action Updated;

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

        public void UpdateView(float duration, float target)
        {
            TargetValue = target;
            UpdateView(duration);
        }

        public void UpdateView(float duration)
        {
            if (isActiveAndEnabled == false)
                return;

            if (Routine != null)
                StopCoroutine(Routine);

            Routine = StartCoroutine(UpdateRoutine(duration));
        }

        private protected virtual IEnumerator UpdateRoutine(float duration)
        {
            OnRoutineStart();
            ElapsedTime = 0;

            while (ElapsedTime < duration)
            {
                if (IsPaused)
                {
                    yield return null;
                    continue;
                }

                ElapsedTime += Time.deltaTime;
                OnRoutineIteration(duration);
                yield return null;
            }

            Routine = null;
            OnRoutineEnd();
        }

        private protected virtual void OnUpdate()
        {
            if (isActiveAndEnabled == false)
                return;

            if (Routine != null)
                StopCoroutine(Routine);

            Routine = StartCoroutine(UpdateRoutine(Duration));
        }

        private protected virtual void OnRoutineStart() { }
        private protected virtual void OnRoutineIteration(float cycleDuration) { }
        private protected virtual void OnRoutineEnd()
        {
            Updated?.Invoke();
        }
    }
}