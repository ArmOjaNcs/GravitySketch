using Assets.Sources.Pause;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class SmoothedImage : PauseableRoutine
    {
        [SerializeField] private protected Image Image;

        private protected float StartImageValue;

        public Color Color => Image.color;

        public void SetValue(float value)
        {
            value = Mathf.Clamp01(value);
            Image.fillAmount = value;
        }

        public void UpdateValue(float duration, float targetValue) => UpdateView(duration, targetValue);

        private protected override void OnRoutineStart()
        {
            StartImageValue = Image.fillAmount;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;
            Image.fillAmount = Mathf.Lerp(StartImageValue, TargetValue, progress);
        }

        private protected override void OnRoutineEnd()
        {
            Image.fillAmount = TargetValue;
            base.OnRoutineEnd();
        }
    }
}