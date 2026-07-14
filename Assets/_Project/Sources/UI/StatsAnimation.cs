using Assets.Sources.Pause;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class StatsAnimation : PauseableRoutine
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private StatUI _shakeAnimation;
        [SerializeField] private Color _imageAnimationColor;
        [SerializeField] private Color _textAnimationColor;

        private float _halfOfDuration;
        private float _progress;
        private Color _startImageColor;
        private Color _startTextColor;
        private Color _imageDefaultColor;
        private Color _textDefaultColor;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _shakeAnimation.Init(pauseHandler);
            _imageDefaultColor = _image.color;
            _textDefaultColor = _text.color;

            if (Mathf.Approximately(Duration, 0))
                Duration = 0.7f;

            _halfOfDuration = Duration / 2;
            _startTextColor = _text.color;
        }

        public void SetText(string text) => _text.text = text;

        public void Play(float duration)
        {
            UpdateView(duration);
            _shakeAnimation.Play();
            _startImageColor = _image.color;
            _startTextColor = _text.color;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            if (ElapsedTime < _halfOfDuration)
            {
                _progress = ElapsedTime / _halfOfDuration;
                _image.color = Color.Lerp(_startImageColor, _imageAnimationColor, _progress);
                _text.color = Color.Lerp(_startTextColor, _textAnimationColor, _progress);
            }
            else
            {
                _progress = (ElapsedTime - _halfOfDuration) / _halfOfDuration;
                _image.color = Color.Lerp(_imageAnimationColor, _imageDefaultColor, _progress);
                _text.color = Color.Lerp(_textAnimationColor, _textDefaultColor, _progress);
            }
        }

        private protected override void OnRoutineEnd()
        {
            _image.color = _imageDefaultColor;
            _text.color = _textDefaultColor;
            base.OnRoutineEnd();
        }
    }
}