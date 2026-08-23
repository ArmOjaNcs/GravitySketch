using System;
using Audio;
using Pause;
using Utils;
using UnityEngine;

namespace PlayerScripts.Ability
{
    public class Shield : PlayerAbility
    {
        [SerializeField]
        [Min(0)] private float _defendUpgradeDelta;
        [SerializeField]
        [Min(2)] private float _minReloadTime;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private AudioPlayer _audioPlayer;

        private bool _isDefended;
        private bool _isDefendApplied;
        private float _defence = 5;

        public event Action DefendApplied;
        public event Action Reloading;

        public float CycleTime { get; private set; }
        public float DefendTime => ActiveTime;
        public bool IsDefended => _isDefended;
        public bool IsReloading { get; private set; }
        public float Defence => _defence;

        private void OnEnable()
        {
            Input.Defended += OnDefended;
        }

        private void OnDisable()
        {
            Input.Defended -= OnDefended;
        }

        private void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            if (_isDefendApplied)
                PlayCycle();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            CycleTime = ReloadTime + ActiveTime;
            _effect.Stop();
            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();
            _effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (IsDefended)
                _effect.Play();
        }

        public override void Upgrade()
        {
            ActiveTime += _defendUpgradeDelta;
            ReloadTime -= ReloadUpgradeDelta;

            if (ReloadTime < _minReloadTime)
                ReloadTime = _minReloadTime;

            CycleTime = ReloadTime + ActiveTime;
        }

        public void UpgradeActiveTime()
        {
            ActiveTime += _defendUpgradeDelta * 4;
            CycleTime = ReloadTime + ActiveTime;
        }

        public void UpgradeDefend()
        {
            if (_defence < 75)
                _defence += UserUtils.Unit;
        }

        private void OnDefended()
        {
            if (_isDefendApplied)
                return;

            _audioPlayer.Play();
            _isDefendApplied = true;
            _isDefended = true;
            _effect.Play();
            DefendApplied?.Invoke();
        }

        private void PlayCycle()
        {
            CurrentActiveTime += Time.deltaTime;

            if (CurrentActiveTime > ActiveTime && IsReloading == false)
            {
                _audioPlayer.Stop();
                _effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _isDefended = false;
                IsReloading = true;
                Reloading?.Invoke();
            }

            if (IsReloading)
            {
                CurrentReloadTime += Time.deltaTime;

                if (CurrentReloadTime > CycleTime - DefendTime)
                {
                    _isDefendApplied = false;
                    IsReloading = false;
                    CurrentReloadTime = 0;
                    CurrentActiveTime = 0;
                }
            }
        }
    }
}