using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Shield : PlayerAbility
    {
        [SerializeField, Min(0)] private float _defendUpgradeDelta;
        [SerializeField, Min(2)] private float _minReloadTime;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private AudioPlayer _audioPlayer;

        private bool _isDefended;
        private bool _isDefendApplied;
        private float _defence;
        private int _defenceThreshold = 10;

        public event Action DefendApplied;
        public event Action Reloading;

        public float CycleTime { get; private set; }
        public float DefendTime => ActiveTime;
        public bool IsDefended => _isDefended;
        public bool IsReloading { get; private set; }
        public float Defence => _defence / _defenceThreshold;

        private void Awake()
        {
            _defence = 10;
        }

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
            ReloadTime -= ReloadUpgradeDelta;
            
            if(ReloadTime < _minReloadTime)
                ReloadTime = _minReloadTime;

            CycleTime = ReloadTime + ActiveTime;
        }

        public void UpgradeActiveTime()
        {
            ActiveTime += _defendUpgradeDelta * 2;
            CycleTime = ReloadTime + ActiveTime;
        }

        public void UpgradeDefend() => _defence += UserUtils.One;
    }
}