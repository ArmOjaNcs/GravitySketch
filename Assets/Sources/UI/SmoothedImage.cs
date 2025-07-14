using Assets.Sources.Pause;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class SmoothedImage : PauseableRoutine
    {
        [SerializeField] private protected Image Image;

        public event Action Updated;

        public Color Color => Image.color;

        public void SetValue(float value)
        {
            value = Mathf.Clamp01(value);
            Image.fillAmount = value;
        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            float elapsedTime = 0;
            float startSliderValue = Image.fillAmount;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                if (CurrentTime < elapsedTime)
                    CurrentTime = elapsedTime;

                float progress = elapsedTime / duration;
                Image.fillAmount = Mathf.Lerp(startSliderValue, TargetValue, progress);

                yield return null;
            }

            Image.fillAmount = TargetValue;
            Routine = null;
            CurrentTime = 0;
            Updated?.Invoke();
        }
    }
}