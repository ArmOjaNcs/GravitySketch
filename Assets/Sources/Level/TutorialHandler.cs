using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class TutorialHandler : MonoBehaviour
    {
        [SerializeField] private TutorialTrigger[] _triggers;
        [SerializeField] private Tutorial[] _tutorials;

        private Tutorial _currentTutorial;

        public event Action Triggered;
        public event Action TutorialViewClosed;

        private void OnEnable()
        {
            foreach (var trigger in _triggers)
                trigger.PlayerInZone += OnPlayerInZone;
        }

        private void OnDisable()
        {
            foreach (var trigger in _triggers)
                trigger.PlayerInZone -= OnPlayerInZone;
        }

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
    }
}