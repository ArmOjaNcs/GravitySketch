using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class PopUpText : PauseableObject
    {
        [SerializeField] private float _duration;
        [SerializeField] private SmoothedFade _smoothedFade;
        [SerializeField] private Color _positiveDifference;
        [SerializeField] private Color _negativeDifference;
        [SerializeField] private float _yPosition;
        [SerializeField] private TextMeshProUGUI _text;

        private float _previousValue;
        private Tween _popUpAnimation;

        public float Difference { get; private set; }

        private protected override void Awake()
        {
            base.Awake();
            _popUpAnimation = AnimationSpawner.
                GetPopUpAnimation(_text.rectTransform, _yPosition, _duration);
        }

        private void OnDisable()
        {
            _popUpAnimation.Kill();
        }

        private void Start()
        {
            _text.text = "";
            _smoothedFade.FadeOut();
        }

        public override void Pause()
        {
            base.Pause();

            if (_popUpAnimation.IsPlaying())
                _popUpAnimation.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_popUpAnimation.IsComplete() == false)
                _popUpAnimation.Play();
        }

        public void ShowText(float currentValue)
        {
            if (IsPaused)
                return;

            _smoothedFade.ShowElements();
            CalculateDifference(currentValue);
            SetDifferenceText();
            _popUpAnimation.Restart();
            _popUpAnimation.OnComplete(() => _smoothedFade.FadeOut());
        }

        public void SetPreviousValue(float value)
        {
            if (value < 0)
                return;

            _previousValue = value;
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
                _text.text = $"+{Difference}";
                _text.color = _positiveDifference;
            }
            else
            {
                _text.text = $"{Difference}";
                _text.color = _negativeDifference;
            }
        }
    }
}