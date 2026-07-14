using Assets.Sources.Pause;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class SmoothedText : PauseableRoutine
    {
        [SerializeField] private protected TextMeshProUGUI Text;

        protected float StartValue;
        protected float CurrentValue;
        protected float MaxValue;
        protected char SplitSign;
        protected bool IsNeedToSplit;

        public void SetColor(Color color) => Text.color = color;

        private protected override void OnRoutineStart()
        {
            StartValue = CurrentValue;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;
            CurrentValue = Mathf.Lerp(StartValue, TargetValue, progress);

            UpdateText();
        }

        private protected override void OnRoutineEnd()
        {
            CurrentValue = TargetValue;
            UpdateText();
            base.OnRoutineEnd();
        }

        private void UpdateText()
        {
            int value = Mathf.RoundToInt(CurrentValue);

            if (IsNeedToSplit && MaxValue > 0)
                Text.SetText("{0}{1}{2}", value + SplitSign + MaxValue);
            else if (MaxValue > 0)
                Text.SetText("{0}{1}", value, MaxValue);
            else
                Text.SetText("{0}", value);
        }
    }
}