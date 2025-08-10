using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class AppearingText : PauseableRoutine
    {
        [SerializeField] private TextMeshProUGUI _text;

        private string _currentText = string.Empty;
        private int _currentIndex;
        private float _step;

        public event Action SignAdded;

        private protected override void OnRoutineStart()
        {
            _currentText = string.Empty;
            _text.text = _currentText;
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            if(ElapsedTime > _currentIndex * _step)
            {
                _currentIndex = Mathf.Clamp(_currentIndex, 0, UserUtils.Loading.Length - 1);
                _currentText += UserUtils.Loading[_currentIndex];
                _text.text = _currentText;
                _currentIndex++;
                SignAdded?.Invoke();
            }
        }

        private protected override void OnRoutineEnd()
        {
            _currentIndex = 0;
            base.OnRoutineEnd();
        }

        public override void UpdateView(float duration)
        {
            _step = duration / UserUtils.Loading.Length;
            base.UpdateView(duration);
        }
    }
}