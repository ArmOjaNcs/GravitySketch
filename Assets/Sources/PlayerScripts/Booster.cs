using Assets.Sources.Audio;
using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Booster : PlayerAbility
    {
        [SerializeField, Min(0)] private int _boostCount;
        [SerializeField] private AudioPlayer _audioPlayer;
        [SerializeField] private ParticleSystem _effect;

        private bool _isBoostApplied;
        private bool _isReloading;

        public event Action Applied;
        public event Action Discarded;
        public event Action CountChanged;
        public event Action Reloading;
        public event Action Reloaded;

        public int CurrentBoostCount { get; private set; }
        public float BoostTime => ActiveTime;
        public float BoostReloadTime => ReloadTime;
        public int BoostCount => _boostCount;

        private void OnEnable()
        {
            Input.Boosted += OnBoosted;
        }

        private void OnDisable()
        {
            Input.Boosted -= OnBoosted;
        }

        private void Update()
        {
            if(IsPaused || IsInitialized == false) 
                return;

            Boost();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
            CurrentBoostCount = _boostCount;
            _effect.Stop();
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();

            if(_effect.isPlaying)
                _effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_isBoostApplied)
                _effect.Play();
        }

        private void Boost()
        {
            if (CurrentBoostCount < _boostCount)
            {
                if (_isReloading == false)
                {
                    _isReloading = true;
                    Reloading?.Invoke();
                }

                CurrentReloadTime += Time.deltaTime;

                if (CurrentReloadTime > ReloadTime)
                    ReloadBoost();
            }
       
            if (_isBoostApplied)
            {
                CurrentActiveTime += Time.deltaTime;

                if (CurrentActiveTime > ActiveTime)
                    StopBoost();
            }
        }

        private void OnBoosted() => TryStartBoost();

        private void TryStartBoost()
        {
            if (_isBoostApplied == false && CurrentBoostCount > 0)
                ApplyBoost();
        }

        private void StopBoost()
        {
            _audioPlayer.Stop();
            _isBoostApplied = false;
            CurrentActiveTime = 0;
            Discarded?.Invoke();
            _effect.Stop();
        }

        private void ReloadBoost()
        {
            CurrentBoostCount++;
            CountChanged?.Invoke();
            Reloaded?.Invoke();
            _isReloading = false;
            CurrentReloadTime = 0;
        }

        private void ApplyBoost()
        {
            _audioPlayer.Play();
            CurrentBoostCount--;
            CountChanged?.Invoke();
            _isBoostApplied = true;
            Applied?.Invoke();
            _effect.Play();
        }

        public override void Upgrade() => ReloadTime -= ReloadUpgradeDelta;
    }
}