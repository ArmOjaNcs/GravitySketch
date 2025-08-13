using System;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class PauseInput : MonoBehaviour
    {
        private bool _isStarted;
        public event Action Paused;

        private bool IsPaused => Input.GetKeyUp(KeyCode.Escape);

        private void Update()
        {
            if (_isStarted == false)
                return;

            if(IsPaused)
                Paused?.Invoke();
        }

        public void StartInput() => _isStarted = true;
        public void StopInput() => _isStarted = false;
    }
}