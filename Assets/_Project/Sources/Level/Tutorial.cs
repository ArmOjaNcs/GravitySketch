using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private TutorialType _type;
        [SerializeField] private TutorialView[] _views;

        private bool _isShown;
        private int _index;
        private TutorialView _currentView;

        public event Action Closed;

        public TutorialType Type => _type;

        public bool IsShown => _isShown;

        public void Show()
        {
            if (_views.Length == 0)
                return;

            if (TryFindViewByIndex(0, out TutorialView view))
                SetNewView(view);
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
            _index++;

            if (TryFindViewByIndex(_index, out TutorialView view))
                SetNewView(view);
            else
                Closed?.Invoke();
        }

        private bool TryFindViewByIndex(int index, out TutorialView view)
        {
            foreach (TutorialView tutorialView in _views)
            {
                if (index == tutorialView.Index)
                {
                    view = tutorialView;
                    return true;
                }
            }

            view = null;
            return false;
        }

        private void SetNewView(TutorialView view)
        {
            _currentView = view;
            _currentView.Closing += OnCurrentViewClosing;
            _currentView.Show();
        }
    }
}