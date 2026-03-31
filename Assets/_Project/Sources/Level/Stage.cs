using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Save;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private protected MenuWindow TextWindow;
        [SerializeField] private protected MenuWindow[] Buttons;
        [SerializeField] private protected AudioClip ButtonSound;
        [SerializeField] private protected AudioClip FinalSound;
        [SerializeField] private protected AudioClip ToggleSound;
        [SerializeField] private protected PauseMenuAnimator PauseMenuAnimator;
        [SerializeField] private protected PauseInput PauseInput;
        [SerializeField] private protected EventSystem EventSystem;

        private protected PauseHandler PauseHandler;
        private protected AudioPlayerSpawner AudioPlayerSpawner;
        private protected InputBindings Bindings;
        private protected GameObject TutorialObject;
        private bool _isStarted;
        private protected bool IsMenuToSubscribe;

        private protected virtual void OnEnable()
        {
            ToMainMenu.onClick.AddListener(OnMainMenuApplied);
            Restart.onClick.AddListener(OnRestartApplied);
            Back.onClick.AddListener(OnBackApplied);
            UseVirtualJoystick.onValueChanged.AddListener(OnVirtualJoystickValueChanged);
            PauseInput.Paused += OnPaused;
        }

        private protected virtual void OnDisable()
        {
            ToMainMenu.onClick.RemoveListener(OnMainMenuApplied);
            Restart.onClick.RemoveListener(OnRestartApplied);
            Back.onClick.RemoveListener(OnBackApplied);
            UseVirtualJoystick.onValueChanged.RemoveListener(OnVirtualJoystickValueChanged);
            PauseInput.Paused -= OnPaused;
        }

        public virtual void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            PauseHandler = pauseHandler;
            AudioPlayerSpawner = audioPlayerSpawner;
            Bindings = SaveSystem.LoadInputBindings();
            UseVirtualJoystick.isOn = Bindings.UseJoystick;
            Back.interactable = false;
            Pause.interactable = true;
        }

        public virtual void Begin()
        {
            PauseInput.StartInput();
            OnVirtualJoystickValueChanged(Bindings.UseJoystick);
            _isStarted = true;
        }

        public virtual void SetTutorialObject(GameObject tutorialObject) => TutorialObject = tutorialObject;

        private protected void Finish()
        {
            Pause.interactable = false;
            PauseInput.StopInput();
            PauseInput.Paused -= OnPaused;
        }

        private protected virtual void OnMainMenuApplied()
        {
            if (TextWindow.IsShown == false)
            {
                Back.interactable = false;
                IsMenuToSubscribe = true;
            }

            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ButtonSound)?.Play();
            Finish();
            Buttons[0].Closed += OnMainMenu;
            HideButtons();
        }

        private void OnMainMenu()
        {
            Buttons[0].Closed -= OnMainMenu;

            if (IsMenuToSubscribe)
            {
                PauseMenuAnimator.Hidden += LoadMainMenu;
                PauseMenuAnimator.Hide();
            }
            else
            {
                TextWindow.Closed += LoadMainMenu;
                TextWindow.Hide();
            }
        }

        private void LoadMainMenu()
        {
            if(IsMenuToSubscribe)
                PauseMenuAnimator.Hidden -= LoadMainMenu;
            else
                TextWindow.Closed -= LoadMainMenu;

            IsMenuToSubscribe = false;
            Progress.SetSceneType(SceneType.Main);
            SaveSystem.SavePlayerProgress(Progress);
            SceneManager.LoadScene(UserUtils.Load);
        }

        private protected virtual void OnRestartApplied()
        {
            if(TextWindow.IsShown == false)
            {
                Back.interactable = false;
                IsMenuToSubscribe = true;
            }

            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ButtonSound)?.Play();
            Finish();
            Buttons[0].Closed += OnRestart;
            HideButtons();
        }

        private void OnRestart()
        {
            Buttons[0].Closed -= OnRestart;

            if (IsMenuToSubscribe)
            {
                PauseMenuAnimator.Hidden += RestartStage;
                PauseMenuAnimator.Hide();
            }
            else
            {
                TextWindow.Closed += RestartStage;
                TextWindow.Hide();
            }
        }

        private protected void OnPaused()
        {
            if(Pause.interactable)
                Pause.interactable = false;

            if (PauseHandler.IsPaused)
            {
                if (PauseMenuAnimator.IsShown)
                {
                    if(Back.interactable)
                        Back.interactable = false;

                    if (Buttons[0].IsShown)
                    {
                        Buttons[0].Closed += HideMenu;
                        HideButtons();
                    }
                }
            }
            else
            {
                PauseHandler.Pause();

                if (PauseMenuAnimator.IsShown == false)
                {
                    PauseMenuAnimator.Shown += OnPauseMenuShown;
                    PauseMenuAnimator.Show();
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
            }
        }

        private void OnPauseMenuShown()
        {
            PauseMenuAnimator.Shown -= OnPauseMenuShown;
            Back.interactable = true;
            ShowButtons();
        }

        private void HideMenu()
        {
            Buttons[0].Closed -= HideMenu;
            PauseMenuAnimator.Hidden += OnPauseMenuClosed;
            Back.interactable = false;
            PauseMenuAnimator.Hide();
        }

        private void OnBackApplied()
        {
            Back.interactable = false;
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

        private protected void ShowButtons()
        {
            foreach (MenuWindow button in Buttons)
                button.Show();
        }

        private protected virtual void HideButtons()
        {
            foreach(MenuWindow button in Buttons)
                button.Hide();
        }

        private protected IEnumerator RefreshEventSystem(Action onComplete)
        {
            yield return new WaitForEndOfFrame();
            EventSystem.enabled = false;
            EventSystem.enabled = true;
            onComplete();
        }

        private void RestartStage()
        {
            if (IsMenuToSubscribe)
                PauseMenuAnimator.Hidden -= RestartStage;
            else
                TextWindow.Closed -= RestartStage;

            IsMenuToSubscribe = false;
            SceneManager.LoadScene(UserUtils.Load);
        }

        private void OnPauseMenuClosed() 
        {
            PauseMenuAnimator.Hidden -= OnPauseMenuClosed;
            Pause.interactable = true;
            PauseHandler.Resume();
        } 
    }
}