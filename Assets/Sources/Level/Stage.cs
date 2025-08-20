using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public abstract class Stage : LevelScore
    {
        [SerializeField] private protected Button ToMainMenu;
        [SerializeField] private protected Button Restart;
        [SerializeField] private protected MenuWindow Window;
        [SerializeField] private protected AudioClip ButtonSound;
        [SerializeField] private PauseMenu _pauseMenu;

        private protected PauseHandler PauseHandler;
        private protected AudioPlayerSpawner AudioPlayerSpawner;

        public event Action Finished;

        private protected virtual void OnEnable()
        {
            ToMainMenu.onClick.AddListener(OnMainMenuApplied);
            Restart.onClick.AddListener(OnRestartApplied);
            _pauseMenu.Opening += OnPauseMenuOpening;
            _pauseMenu.Closing += OnPauseMenuClosing;
        }

        private protected virtual void OnDisable()
        {
            ToMainMenu.onClick.RemoveListener(OnMainMenuApplied);
            Restart.onClick.RemoveListener(OnRestartApplied);
            _pauseMenu.Opening -= OnPauseMenuOpening;
            _pauseMenu.Closing -= OnPauseMenuClosing;
        }

        public virtual void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            PauseHandler = pauseHandler;
            AudioPlayerSpawner = audioPlayerSpawner;
        }

        private protected void HidePauseMenu()
        {
            if (_pauseMenu.IsShown)
                _pauseMenu.Hide();
        }

        private protected void InvokeFinished() => Finished?.Invoke();

        private void OnMainMenuApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(ButtonSound).Play();
            Finished?.Invoke();
            Window.Closed += LoadMainMenu;
            Window.Hide();
            HidePauseMenu();
        }

        private void LoadMainMenu()
        {
            Window.Closed -= LoadMainMenu;
            LoadScene(UserUtils.MainMenu);
        }

        private void OnRestartApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(ButtonSound).Play();
            Finished?.Invoke();
            Window.Closed += RestartStage;
            Window.Hide();
            HidePauseMenu();
        }

        private void RestartStage()
        {
            Window.Closed -= RestartStage;
            RestartScene();
        }

        private void OnPauseMenuOpening() => Window.Show();
        private void OnPauseMenuClosing() => Window.Hide();
    }
}