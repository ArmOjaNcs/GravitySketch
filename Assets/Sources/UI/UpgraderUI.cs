using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class UpgraderUI : PauseableObject
    {
        [SerializeField] private Upgrader _upgrader;
        [SerializeField] private StatsAnimation _powerAnimation;
        [SerializeField] private StatsAnimation _sizeAnimation;

        private int _previousSize;

        private void OnEnable()
        {
            _upgrader.Upgraded += OnUpgraded;
        }

        private void OnDisable()
        {
            _upgrader.Upgraded -= OnUpgraded;  
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _powerAnimation.Init(pauseHandler);
            _sizeAnimation.Init(pauseHandler);
            _powerAnimation.SetText(_upgrader.Power.ToString());
            _sizeAnimation.SetText(_upgrader.CurrentSize.ToString());
            _previousSize = _upgrader.CurrentSize;
            IsInitialized = true;
        }

        private void OnUpgraded()
        {
            _powerAnimation.SetText(_upgrader.Power.ToString());
            _powerAnimation.UpdateView(2);

            if(_previousSize < _upgrader.CurrentSize)
            {
                _previousSize = _upgrader.CurrentSize;
                _sizeAnimation.SetText(_upgrader.CurrentSize.ToString());
                _sizeAnimation.UpdateView(2);
            }
        }
    }
}