using Assets.Sources.Pause;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class AppearingText : PauseableRoutine
    {
        [SerializeField] private TextMeshProUGUI _text;

        private string _totalText = string.Empty;
        private string _currentText = string.Empty;
        private int _currentIndex;
        private float _step;

        private protected override void OnRoutineStart()
        {
            _currentText = string.Empty;
            _text.text = _currentText;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            if(ElapsedTime > _currentIndex * _step)
            {
                _currentIndex = Mathf.Clamp(_currentIndex, 0, _totalText.Length - 1);
                _currentText += _totalText[_currentIndex];
                _text.text = _currentText;
                _currentIndex++;
            }
        }

        private protected override void OnRoutineEnd()
        {
            _currentIndex = 0;
            base.OnRoutineEnd();
        }

        public void Play(float duration)
        {
            if (_totalText == string.Empty)
                _totalText = Translator.Get(_text.text);

            _step = duration / (_totalText.Length - 1);
            UpdateView(duration);
        }
    }
}