using Assets.Sources.Pause;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class SmoothedText : PauseableRoutine
    {
        [SerializeField] private protected TextMeshProUGUI Text;

        private protected string StartText;
        private protected string EndText;
        private protected float StartValue;
        private protected float CurrentValue;
        private protected float MaxValue;
        private protected char SplitSign;
        private protected bool IsNeedToSplit;

        public void SetColor(Color color) => Text.color = color;

        private float ParseCurrentTextValue()
        {
            string text = RemoveAdditionalText();
            string textToParse = string.Empty;

            if (IsNeedToSplit)
            {
                string[] textParts = text.Split(SplitSign);
                textToParse = textParts[0];
            }
            else
            {
                textToParse = text;
            }

            if (textToParse.Length > 0 && float.TryParse(textToParse, out float result))
                return result;
            else
                return 0;
        }

        private string RemoveAdditionalText()
        {
            string text = Text.text;

            if (StartText != null && StartText.Length > 0)
                text = text.Replace(StartText, "").Trim();

            if (EndText != null && EndText.Length > 0)
                text = text.Replace(EndText, "").Trim();

            return text;
        }

        private protected string GetTotalText()
        {
            string totalText = string.Empty;

            if (StartText != null)
                totalText += StartText;

            totalText += Mathf.Round(CurrentValue).ToString();

            if (SplitSign != UserUtils.DefaultChar)
                totalText += SplitSign;

            if (MaxValue > 0)
                totalText += MaxValue;

            if (EndText != null)
                totalText += EndText;

            return totalText;
        }

        private protected override void OnRoutineStart()
        {
            StartValue = ParseCurrentTextValue();
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;
            CurrentValue = Mathf.Lerp(StartValue, TargetValue, progress);
            Text.text = GetTotalText();
        }

        private protected override void OnRoutineEnd()
        {
            CurrentValue = TargetValue;
            Text.text = GetTotalText();
            base.OnRoutineEnd();
        }
    }
}