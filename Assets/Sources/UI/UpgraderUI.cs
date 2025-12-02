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
        [SerializeField] private AudioPlayer _audioPlayer;

        private Dictionary<StatsAnimationType, StatsAnimation> _animationsByType = new();
        private bool _isStarted;

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
            _audioPlayer.Init(pauseHandler);
           
            foreach(StatsAnimation statsAnimation in _statsAnimations)
            {
                _animationsByType.Add(statsAnimation.Type, statsAnimation);
                statsAnimation.Init(pauseHandler);
            }

            foreach (StatsAnimation statsAnimation in _statsAnimations)
                OnUpgraded(statsAnimation.Type);

            IsInitialized = true;
        }

        public void GrowUp()
        {
            _audioPlayer.Play();
        }

        private void OnUpgraded(StatsAnimationType animationType)
        {
            switch (animationType)
            {
                case StatsAnimationType.MoveSpeed:
                    _animationsByType[animationType].SetText(_upgrader.MoveSpeed.ToString("F1"));
                    _animationsByType[animationType].UpdateView(2);
                    break;
               
                case StatsAnimationType.DefenceTime:
                    _animationsByType[animationType].SetText(_upgrader.DefendTime.ToString("F2"));
                    _animationsByType[animationType].UpdateView(2);
                    break;

                case StatsAnimationType.Defence:
                    _animationsByType[animationType].SetText(_upgrader.Defence.ToString() + '%');
                    _animationsByType[animationType].UpdateView(2);
                    break;

                case StatsAnimationType.Damage:
                    float damagePerSecond = _upgrader.Damage * 2;
                    _animationsByType[animationType].SetText(damagePerSecond.ToString());
                    _animationsByType[animationType].UpdateView(2);
                    break;

                case StatsAnimationType.Size:
                    _animationsByType[animationType].SetText(_upgrader.CurrentSize.ToString());
                    _animationsByType[animationType].UpdateView(2);

                    if(_isStarted)
                        _audioPlayer.Play();
                    else
                        _isStarted = true;

                    break;

                default:
                    break;
            }
        }
    }
}