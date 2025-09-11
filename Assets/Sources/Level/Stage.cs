using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public abstract class Stage : LevelScore
    {
        [SerializeField] private protected Button ToMainMenu;
        [SerializeField] private protected Button Restart;
        [SerializeField] private protected MenuWindow Window;
        [SerializeField] private protected AudioClip ButtonSound;
        [SerializeField] private protected AudioClip FinalSound;
        [SerializeField] private PauseMenuAnimator _pauseMenuAnimator;
        [SerializeField] private PauseInput _pauseInput;

        private protected PauseHandler PauseHandler;
        private protected AudioPlayerSpawner AudioPlayerSpawner; 

        private protected virtual void OnEnable()
        {
            ToMainMenu.onClick.AddListener(OnMainMenuApplied);
            Restart.onClick.AddListener(OnRestartApplied);
            _pauseMenuAnimator.Hidden += OnPauseMenuClosed;
            _pauseInput.Paused += OnPaused;
        }

        private protected virtual void OnDisable()
        {
            ToMainMenu.onClick.RemoveListener(OnMainMenuApplied);
            Restart.onClick.RemoveListener(OnRestartApplied);
            _pauseMenuAnimator.Hidden -= OnPauseMenuClosed;
            _pauseInput.Paused -= OnPaused;
        }

        public virtual void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            PauseHandler = pauseHandler;
            AudioPlayerSpawner = audioPlayerSpawner;
        }

        public void Begin()
        {
            _pauseInput.StartInput();
        }

        private protected void HidePauseMenu()
        {
            if (_pauseMenuAnimator.IsShown)
                _pauseMenuAnimator.BaseHide();
        }

        private protected void Finish()
        {
            _pauseInput.StopInput();
            _pauseInput.Paused -= OnPaused;
        }

        private protected virtual void OnMainMenuApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(ButtonSound).Play();
            Finish();
            Window.Closed += LoadMainMenu;
            Window.Hide();
            HidePauseMenu();
        }

        private void LoadMainMenu()
        {
            Window.Closed -= LoadMainMenu;
            SceneManager.LoadScene(UserUtils.MainMenu);
        }

        private protected virtual void OnRestartApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(ButtonSound).Play();
            Finish();
            Window.Closed += RestartStage;
            Window.Hide();
            HidePauseMenu();
        }

        private void OnPaused()
        {
            if (PauseHandler.IsPaused)
            {
                if (_pauseMenuAnimator.IsShown)
                {
                    _pauseMenuAnimator.Hide();
                    Window.Hide();
                }
            }
            else
            {
                PauseHandler.Pause();

                if(_pauseMenuAnimator.IsShown == false)
                {
                    _pauseMenuAnimator.Show();
                    Window.Show();
                }
            }
        }

        private void RestartStage()
        {
            Window.Closed -= RestartStage;
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        private void OnPauseMenuClosed() => PauseHandler.Resume();
    }
}