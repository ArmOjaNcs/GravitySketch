using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class UpgraderUI : PauseableObject
    {
        [SerializeField] private Upgrader _upgrader;
        [SerializeField] private StatsAnimation[] _statsAnimations;

        private Dictionary<StatsAnimationType, StatsAnimation> _animationsByType = new();

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
           
            foreach(StatsAnimation statsAnimation in _statsAnimations)
            {
                _animationsByType.Add(statsAnimation.Type, statsAnimation);
                statsAnimation.Init(pauseHandler);
            }

            foreach (StatsAnimation statsAnimation in _statsAnimations)
                SetText(statsAnimation.Type);

            IsInitialized = true;
        }

        private void OnUpgraded(StatsAnimationType animationType)
        {
            SetText(animationType);
            _animationsByType[animationType].UpdateView(2);
        }

        private void SetText(StatsAnimationType animationType)
        {
            switch (animationType)
            {
                case StatsAnimationType.MoveSpeed:
                    _animationsByType[animationType].SetText(_upgrader.MoveSpeed.ToString("F1"));
                    break;

                case StatsAnimationType.DefenceTime:
                    _animationsByType[animationType].SetText(_upgrader.DefendTime.ToString("F2"));
                    break;

                case StatsAnimationType.Defence:
                    _animationsByType[animationType].SetText(_upgrader.Defence.ToString() + '%');
                    break;

                case StatsAnimationType.Damage:
                    float damagePerSecond = _upgrader.Damage * 2;
                    _animationsByType[animationType].SetText(damagePerSecond.ToString());
                    break;

                case StatsAnimationType.Size:
                    _animationsByType[animationType].SetText(_upgrader.CurrentSize.ToString());
                    break;

                default:
                    break;
            }
        }
    }
}