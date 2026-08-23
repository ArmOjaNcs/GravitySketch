using System;
using UI;
using UI.AnimatorMorph;
using UnityEngine;
using UnityEngine.UI;

namespace Level.TutorialScripts
{
    public class TutorialHandler : MonoBehaviour
    {
        [SerializeField] private TutorialTrigger[] _triggers;
        [SerializeField] private Tutorial[] _tutorials;
        [SerializeField] private MenuWindow _startWindow;
        [SerializeField] private Button _accept;
        [SerializeField] private Button _decline;

        private Tutorial _currentTutorial;
        private bool _isAccepted;

        public event Action Triggered;
        public event Action TutorialViewClosed;
        public event Action<bool> IsAccepted;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        public void Begin() => _startWindow.Show();

        public void StartTutorial()
        {
            foreach (TutorialTrigger tutorialTrigger in _triggers)
                tutorialTrigger.EnableCollider();
        }

        public void Show()
        {
            _currentTutorial.Closed += OnCurrentTutorialClosed;
            _currentTutorial.Show();
        }

        private void OnCurrentTutorialClosed()
        {
            _currentTutorial.Closed -= OnCurrentTutorialClosed;
            TutorialViewClosed?.Invoke();
        }

        private void OnPlayerInZone(TutorialType type)
        {
            if (TryFindTutorialByType(type, out Tutorial tutorial))
            {
                _currentTutorial = tutorial;
                Triggered?.Invoke();
            }
        }

        private bool TryFindTutorialByType(TutorialType type, out Tutorial tutorial)
        {
            foreach (Tutorial tutor in _tutorials)
            {
                if (tutor.Type == type && tutor.IsShown == false)
                {
                    tutorial = tutor;

                    foreach (TutorialTrigger tutorialTrigger in _triggers)
                    {
                        if (tutorialTrigger.Type == type)
                            tutorialTrigger.gameObject.SetActive(false);
                    }

                    return true;
                }
            }

            tutorial = null;
            return false;
        }

        private void OnAcceptClicked()
        {
            _isAccepted = true;
            _startWindow.Hide();
        }

        private void OnDeclineClicked()
        {
            _isAccepted = false;
            _startWindow.Hide();
        }

        private void Subscribe()
        {
            foreach (var trigger in _triggers)
                trigger.PlayerInZone += OnPlayerInZone;

            _accept.onClick.AddListener(OnAcceptClicked);
            _decline.onClick.AddListener(OnDeclineClicked);
            _startWindow.Closed += OnStartWindowClosed;
        }

        private void UnSubscribe()
        {
            foreach (var trigger in _triggers)
                trigger.PlayerInZone -= OnPlayerInZone;

            _accept.onClick.RemoveListener(OnAcceptClicked);
            _decline.onClick.RemoveListener(OnDeclineClicked);
            _startWindow.Closed -= OnStartWindowClosed;
        }

        private void OnStartWindowClosed()
        {
            IsAccepted?.Invoke(_isAccepted);
        }
    }
}