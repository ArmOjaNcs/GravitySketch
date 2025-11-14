using Assets.Sources.Audio;
using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Booster : PlayerAbility
    {
        [SerializeField, Min(0)] private float _boostSpeed;
        [SerializeField, Min(0)] private int _boostCount;
        [SerializeField, Min(0)] private float _boostSpeedUpgradeDelta;
        [SerializeField] private AudioPlayer _audioPlayer;

        private bool _isBoostApplied;
        private bool _isReloading;

        public event Action<float> BoostApplied;
        public event Action BoostCountChanged;
        public event Action Reloading;
        public event Action Reloaded;

        public int CurrentBoostCount { get; private set; }
        public float BoostSpeed => _boostSpeed;
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
            IsInitialized = true;
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
            BoostApplied?.Invoke(0);
        }

        private void ReloadBoost()
        {
            CurrentBoostCount++;
            BoostCountChanged?.Invoke();
            Reloaded?.Invoke();
            _isReloading = false;
            CurrentReloadTime = 0;
        }

        private void ApplyBoost()
        {
            _audioPlayer.Play();
            CurrentBoostCount--;
            BoostCountChanged?.Invoke();
            _isBoostApplied = true;
            BoostApplied?.Invoke(_boostSpeed);
        }

        public override void Upgrade()
        {
            _boostSpeed += _boostSpeedUpgradeDelta;
            ReloadTime -= ReloadUpgradeDelta;
        }
    }
}