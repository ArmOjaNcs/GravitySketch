using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class WaitRoutine : PauseableObject
    {
        private Coroutine _routine;
        private float _elapsedTime;

        public event Action Updated;

        private protected virtual void OnDisable()
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = null;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsInitialized = true;
        }

        public void Wait(float durationInSec)
        {
            if (isActiveAndEnabled == false)
                return;

            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(UpdateRoutine(durationInSec));
        }

        private IEnumerator UpdateRoutine(float duration)
        {
            _elapsedTime = 0;

            while (_elapsedTime < duration)
            {
                if (IsPaused)
                {
                    yield return null;
                    continue;
                }

                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            Updated?.Invoke();
            _routine = null;
        }
    }
}