using Assets.Sources.Pause;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.ColorizerScripts
{
    public class ColorReferenceViewHandler : PauseableRoutine
    {
        [SerializeField] private Button _showButton;

        private bool _isAutoPaint;

        public event Action<bool> IsShowing;

        private void OnEnable()
        {
            _showButton.onClick.AddListener(ShowReference);
        }

        private protected override void OnDisable()
        {
            _showButton.onClick.RemoveListener(ShowReference);
            base.OnDisable();
        }

        public void SetAutoPaint(bool autoPaint) => _isAutoPaint = autoPaint;

        private void ShowReference()
        {
            if (_isAutoPaint || Routine != null || IsPaused)
                return;

            OnUpdate();
        }

        private protected override void OnRoutineStart()
        {
            IsShowing?.Invoke(true);
        }

        private protected override void OnRoutineEnd()
        {
            IsShowing?.Invoke(false);
        }
    }
}