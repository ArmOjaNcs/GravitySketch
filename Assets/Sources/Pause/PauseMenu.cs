using Assets.Sources.UI;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private PauseMenuAnimator _animator;
       
        private PauseInput _pauseInput;
        private PauseHandler _pauseHandler;

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

        private void OnPaused()
        {
            if (_pauseHandler.IsPaused)
            {
                if (_animator.IsShown)
                    _animator.Hide();
                else
                    _animator.Show();
            }
            else
            {
                _pauseHandler.Pause();
                _animator.Show();
            }
        }

        private void OnHidden()
        {
            _pauseHandler.Resume();
        }
    }
}