using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Save;
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
        [SerializeField] private protected Button Back;
        [SerializeField] private protected Button Pause;
        [SerializeField] private protected Toggle UseVirtualJoystick;
        [SerializeField] private protected MenuWindow Window;
        [SerializeField] private protected AudioClip ButtonSound;
        [SerializeField] private protected AudioClip FinalSound;
        [SerializeField] private protected AudioClip ToggleSound;
        [SerializeField] private PauseMenuAnimator _pauseMenuAnimator;
        [SerializeField] private PauseInput _pauseInput;

        private protected PauseHandler PauseHandler;
        private protected AudioPlayerSpawner AudioPlayerSpawner;
        private protected InputBindings Bindings;
        private protected GameObject TutorialObject;
        private bool _isStarted;

        private protected virtual void OnEnable()
        {
            ToMainMenu.onClick.AddListener(OnMainMenuApplied);
            Restart.onClick.AddListener(OnRestartApplied);
            Back.onClick.AddListener(OnBackApplied);
            UseVirtualJoystick.onValueChanged.AddListener(OnVirtualJoystickValueChanged);
            _pauseMenuAnimator.Hidden += OnPauseMenuClosed;
            _pauseInput.Paused += OnPaused;
        }

        private protected virtual void OnDisable()
        {
            ToMainMenu.onClick.RemoveListener(OnMainMenuApplied);
            Restart.onClick.RemoveListener(OnRestartApplied);
            Back.onClick.RemoveListener(OnBackApplied);
            UseVirtualJoystick.onValueChanged.RemoveListener(OnVirtualJoystickValueChanged);
            _pauseMenuAnimator.Hidden -= OnPauseMenuClosed;
            _pauseInput.Paused -= OnPaused;
        }

        public virtual void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            PauseHandler = pauseHandler;
            AudioPlayerSpawner = audioPlayerSpawner;
            Bindings = SaveSystem.LoadInputBindings();
            UseVirtualJoystick.isOn = Bindings.UseJoystick;
            Pause.interactable = true;
        }

        public virtual void Begin()
        {
            _pauseInput.StartInput();
            OnVirtualJoystickValueChanged(Bindings.UseJoystick);
            _isStarted = true;
        }

        public virtual void SetTutorialObject(GameObject tutorialObject) => TutorialObject = tutorialObject;

        private protected void HidePauseMenu()
        {
            if (_pauseMenuAnimator.IsShown)
                _pauseMenuAnimator.BaseHide();
        }

        private protected void Finish()
        {
            Pause.gameObject.SetActive(false);
            _pauseInput.StopInput();
            _pauseInput.Paused -= OnPaused;
        }

        private protected virtual void OnMainMenuApplied()
        {
            if (IsTutorial)
            {
                Progress.SetTutorial(false);
                SaveSystem.SavePlayerProgress(Progress);
            }

            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ButtonSound)?.Play();
            Finish();
            Window.Closed += LoadMainMenu;
            Window.Hide();
            HidePauseMenu();
        }

        private void LoadMainMenu()
        {
            Window.Closed -= LoadMainMenu;
            Progress.SetSceneType(SceneType.Main);
            SaveSystem.SavePlayerProgress(Progress);
            SceneManager.LoadScene(UserUtils.Load);
        }

        private protected virtual void OnRestartApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ButtonSound)?.Play();
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

                    if (Pause.gameObject.activeSelf)
                        Pause.interactable = true;
                }
            }
            else
            {
                PauseHandler.Pause();

                if (_pauseMenuAnimator.IsShown == false)
                {
                    _pauseMenuAnimator.Show();
                    Window.Show();

                    if (Pause.gameObject.activeSelf)
                        Pause.interactable = false;
                }
            }
        }

        private void OnBackApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ButtonSound)?.Play();
            OnPaused();
        }

        private protected virtual void OnVirtualJoystickValueChanged(bool value)
        {
            if (_isStarted)
                AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ToggleSound)?.Play();

            Bindings.UseJoystick = value;
            SaveSystem.SaveInputBindings(Bindings);
        }

        private void RestartStage()
        {
            Window.Closed -= RestartStage;
            SceneManager.LoadScene(UserUtils.Load);
        }

        private void OnPauseMenuClosed() => PauseHandler.Resume();
    }
}