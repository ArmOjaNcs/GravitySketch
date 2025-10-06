using Assets.Sources.Audio;
using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Shield : PlayerAbility
    {
        [SerializeField, Min(0)] private float _defendUpgradeDelta;
        [SerializeField, Min(1)] private float _maxDefendTime;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private AudioPlayer _audioPlayer;

        private bool _isDefended;
        private bool _isDefendApplied;

        public event Action DefendApplied;
        public event Action Reloading;

        public float CycleTime { get; private set; }
        public float DefendTime => ActiveTime;
        public bool IsDefended => _isDefended;
        public bool IsReloading { get; private set; }

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
            if(IsPaused || IsInitialized == false) 
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

        public override void Upgrade()
        {
            ActiveTime += _defendUpgradeDelta;
            CycleTime -= ReloadUpgradeDelta;
        }
    }
}