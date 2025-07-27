using Assets.Sources.UI;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private PauseInput _pauseInput;
        [SerializeField] private UIAnimator _animator;

        private void OnEnable()
        {
            _pauseInput.Paused += OnPaused;
            _animator.Hidden += OnHidden;
        }

        private void OnDisable()
        {
            _pauseInput.Paused -= OnPaused;
            _animator.Hidden -= OnHidden;
        }

        private void OnPaused()
        {
            if (PauseableObjectsHandler.IsPaused)
            {
                if(_animator.IsShown)
                    _animator.Hide();
                else
                    _animator.Show();
            }
            else
            {
                PauseableObjectsHandler.Pause();
                _animator.Show();
            }
        }

        private void OnHidden()=> PauseableObjectsHandler.Resume();
    }
}