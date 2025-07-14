using Assets.Sources.Pause;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class SmoothedSlider : PauseableRoutine
    {
        [SerializeField] private protected Slider Slider;

        public event Action Updated;

        public void SetStartValue(float value)
        {
            value = Mathf.Clamp01(value);
            Slider.value = value;
        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            float elapsedTime = 0;
            float startSliderValue = Slider.value;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                if (CurrentTime < elapsedTime)
                    CurrentTime = elapsedTime;

                float progress = elapsedTime / duration;
                Slider.value = Mathf.Lerp(startSliderValue, TargetValue, progress);

                yield return null;
            }

            Slider.value = TargetValue;
            Routine = null;
            CurrentTime = 0;
            Updated?.Invoke();
        }
    }
}