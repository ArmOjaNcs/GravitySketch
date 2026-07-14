using System;
using Assets.Sources.Pause;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.ColorizerScripts
{
    public class ColorReferenceViewHandler : PauseableRoutine
    {
        [SerializeField] private Button _showButton;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private AudioSource _buttonSound;
        [SerializeField]
        [Range(1,10)] private int _showCounts;

        private bool _isAutoPaint;

        public event Action<bool> IsShowing;

        public int ShowCount => _showCounts;

        private void OnEnable()
        {
            _showButton.onClick.AddListener(ShowReference);
        }

        private protected override void OnDisable()
        {
            _showButton.onClick.RemoveListener(ShowReference);
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _text.text = _showCounts.ToString();
            IsInitialized = true;
        }

        public void SetAutoPaint(bool autoPaint) => _isAutoPaint = autoPaint;

        private protected override void OnRoutineStart()
        {
            IsShowing?.Invoke(true);
        }

        private protected override void OnRoutineEnd()
        {
            IsShowing?.Invoke(false);
            base.OnRoutineEnd();
        }

        private void ShowReference()
        {
            if (_isAutoPaint || Routine != null || IsPaused || _showCounts == 0)
                return;

            _buttonSound.Play();
            _showCounts--;
            _text.text = _showCounts.ToString();
            OnUpdate();
        }
    }
}