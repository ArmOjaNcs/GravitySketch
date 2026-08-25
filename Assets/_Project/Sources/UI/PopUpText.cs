using DG.Tweening;
using Pause;
using TMPro;
using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;

namespace UI
{
    public class PopUpText : PauseableAnimation
    {
        [SerializeField] private SmoothedFade _smoothedFade;
        [SerializeField] private Color _positiveDifference;
        [SerializeField] private Color _negativeDifference;
        [SerializeField] private float _yPosition;
        [SerializeField] private TextMeshProUGUI _text;

        private float _previousValue;

        public float Difference { get; private set; }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _smoothedFade.Init(pauseHandler);
            _text.SetText(string.Empty);
            _smoothedFade.FadeOut(0);
            IsInitialized = true;
        }

        public void ShowText(float currentValue)
        {
            if (IsPaused || IsInitialized == false)
                return;

            _smoothedFade.FadeIn(UserUtils.FadeDuration, UserUtils.MaxAlpha);
            CalculateDifference(currentValue);
            SetDifferenceText();
            Animation.Restart();
            Animation.OnComplete(() => _smoothedFade.FadeOut(UserUtils.FadeDuration));
        }

        public void SetPreviousValue(float value)
        {
            if (value < 0)
                return;

            _previousValue = value;
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetPopUpAnimation(_text.rectTransform, 50, 1);
        }

        private void CalculateDifference(float currentValue)
        {
            Difference = currentValue - _previousValue;
            _previousValue = currentValue;
        }

        private void SetDifferenceText()
        {
            if (Difference >= 0)
            {
                _text.SetText($"+{Difference}");
                _text.color = _positiveDifference;
            }
            else
            {
                _text.SetText($"{Difference}");
                _text.color = _negativeDifference;
            }
        }
    }
}