using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class TutorialHandler : MonoBehaviour
    {
        [SerializeField] private TutorialTrigger[] _triggers;
        [SerializeField] private TutorialView[] _views;

        private TutorialView _currentView;

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

        private void OnPlayerInZone(TutorialType type)
        {
            foreach(TutorialView tutorialView in _views)
            {
                if (tutorialView.Type == type)
                {
                    _currentView = tutorialView;

                    foreach(TutorialTrigger tutorialTrigger in _triggers)
                    {
                        if(tutorialTrigger.Type == type)
                            tutorialTrigger.gameObject.SetActive(false);
                    }
                        
                    break;
                }
            }
            
            Triggered?.Invoke();
        }

        public void Show()
        {
            _currentView.Closing += OnCurrentViewClosing;
            _currentView.Show();
        }

        public void StartTutorial()
        {
            foreach (TutorialTrigger tutorialTrigger in _triggers)
                tutorialTrigger.EnableCollider();
        }

        private void OnCurrentViewClosing()
        {
            _currentView.Closing -= OnCurrentViewClosing;
            _currentView.Closed += OnCurrentViewClosed;
            _currentView.Hide();
        }

        private void OnCurrentViewClosed()
        {
            _currentView.Closed -= OnCurrentViewClosed;
            TutorialViewClosed?.Invoke();
        }
    }
}