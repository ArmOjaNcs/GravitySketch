using Pause;
using PlayerScripts;
using Utils;
using UnityEngine;

namespace UI.Stats
{
    public class UpgraderUI : PauseableObject
    {
        [SerializeField] private Upgrader _upgrader;
        [SerializeField] private StatsAnimation _powerAnimation;
        [SerializeField] private StatsAnimation _sizeAnimation;

        private int _previousSize;

        private void OnEnable()
        {
            _upgrader.Started += OnUpgraderStarted;
            _upgrader.Upgraded += OnUpgraded;
        }

        private void OnDisable()
        {
            _upgrader.Started -= OnUpgraderStarted;
            _upgrader.Upgraded -= OnUpgraded;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _powerAnimation.Init(pauseHandler);
            _sizeAnimation.Init(pauseHandler);
            _previousSize = _upgrader.CurrentSize;
            IsInitialized = true;
        }

        private void OnUpgraderStarted()
        {
            _powerAnimation.SetText(_upgrader.Power.ToString());
            _sizeAnimation.SetText(_upgrader.CurrentSize.ToString());
        }

        private void OnUpgraded()
        {
            _powerAnimation.SetText(_upgrader.Power.ToString());
            _powerAnimation.Play(UserUtils.Two);

            if (_previousSize < _upgrader.CurrentSize)
            {
                _previousSize = _upgrader.CurrentSize;
                _sizeAnimation.SetText(_upgrader.CurrentSize.ToString());
                _sizeAnimation.Play(UserUtils.Two);
            }
        }
    }
}