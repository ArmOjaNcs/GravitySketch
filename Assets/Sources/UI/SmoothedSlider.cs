using Assets.Sources.Pause;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class SmoothedSlider : PauseableRoutine
    {
        [SerializeField] private protected Slider Slider;

        private protected float StartSliderValue;

        public void SetStartValue(float value)
        {
            value = Mathf.Clamp01(value);
            Slider.value = value;
        }

        private protected override void OnRoutineStart()
        {
            StartSliderValue = Slider.value;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;
            Slider.value = Mathf.Lerp(StartSliderValue, TargetValue, progress);
        }

        private protected override void OnRoutineEnd()
        {
            Slider.value = TargetValue;
            base.OnRoutineEnd();
        }
    }
}