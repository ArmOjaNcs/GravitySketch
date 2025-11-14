using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Pause
{
    public class PauseInput : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton = null;

        private bool _isStarted;
        public event Action Paused;

        private bool IsPaused => Input.GetKeyUp(KeyCode.Escape);

        private void OnEnable()
        {
            if (_pauseButton != null)
                _pauseButton.onClick.AddListener(SetPause);
        }

        private void OnDisable()
        {
            if (_pauseButton != null)
                _pauseButton.onClick.RemoveListener(SetPause);
        }

        private void Update()
        {
            if (_isStarted == false)
                return;

            if(IsPaused)
                Paused?.Invoke();
        }

        public void StartInput() => _isStarted = true;
        public void StopInput() => _isStarted = false;

        private void SetPause()
        {
            if (_isStarted == false)
                return;

            Paused?.Invoke();
        }
    }
}