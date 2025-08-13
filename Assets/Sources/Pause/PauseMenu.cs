using Assets.Sources.UI;
using System;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private PauseMenuAnimator _animator;
       
        private PauseInput _pauseInput;
        private PauseHandler _pauseHandler;

        public event Action Opening;
        public event Action Closing;

        public bool IsShown => _animator.IsShown;

        private void OnDisable()
        {
            _animator.Hidden -= OnHidden;
            _pauseInput.Paused -= OnPaused;
        }

        public void Init(PauseHandler pauseHandler, PauseInput pauseInput)
        {
            _pauseHandler = pauseHandler;
            _pauseInput = pauseInput;
            _animator.Hidden += OnHidden;
            _pauseInput.Paused += OnPaused;
        }

        public void Hide() => _animator.BaseHide();

        private void OnPaused()
        {
            if (_pauseHandler.IsPaused)
            {
                if (_animator.IsShown)
                {
                    _animator.Hide();
                    Closing?.Invoke();
                }
                else
                {
                    _animator.Show();
                    Opening?.Invoke();
                }
            }
            else
            {
                _pauseHandler.Pause();
                _animator.Show();
                Opening?.Invoke();
            }
        }

        private void OnHidden()
        {
            _pauseHandler.Resume();
        }
    }
}