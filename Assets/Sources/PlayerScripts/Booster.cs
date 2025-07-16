using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Booster : PauseableObject
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField, Min(0)] private float _boostSpeed;
        [SerializeField, Min(0)] private int _boostCount;
        [SerializeField, Min(0)] private float _boostTime = 0.7f;
        [SerializeField, Min(0)] private float _boostReloadTime;
        [SerializeField, Min(0)] private float _boostReloadUpgradeDelta;
        [SerializeField, Min(0)] private float _boostSpeedUpgradeDelta;

        private bool _isBoostApplied;
        private bool _isBoosted;
        private bool _isReloading;

        public event Action<float> BoostApplied;
        public event Action BoostCountChanged;
        public event Action Reloading;
        public event Action Reloaded;

        public float CurrentBoostReloadTime { get; private set; }
        public float CurrentBoostTime { get; private set; }
        public int CurrentBoostCount { get; private set; }
        public float BoostSpeed => _boostSpeed;
        public float BoostTime => _boostTime;
        public float BoostReloadTime => _boostReloadTime;
        public int BoostCount => _boostCount;

        private protected override void Awake()
        {
            base.Awake();
            CurrentBoostCount = _boostCount;
        }

        private void OnEnable()
        {
            _playerInput.Boosted += OnBoosted;
        }

        private void OnDisable()
        {
            _playerInput.Boosted -= OnBoosted;
        }

        private void OnBoosted(bool isBoosted) => _isBoosted = isBoosted;

        private void Update()
        {
            if(IsPaused) 
                return;

            Boost();
        }

        public void UpgradeBoost()
        {
            _boostSpeed += _boostSpeedUpgradeDelta;
            _boostReloadTime -= _boostReloadUpgradeDelta;
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

                CurrentBoostReloadTime += Time.deltaTime;

                if (CurrentBoostReloadTime > _boostReloadTime)
                    ReloadBoost();
            }

            if (_isBoosted && _isBoostApplied == false && CurrentBoostCount > 0)
                ApplyBoost();

            if (_isBoostApplied)
            {
                CurrentBoostTime += Time.deltaTime;

                if (CurrentBoostTime > _boostTime)
                    StopBoost();
            }
        }

        private void StopBoost()
        {
            _isBoostApplied = false;
            CurrentBoostTime = 0;
            BoostApplied?.Invoke(0);
        }

        private void ReloadBoost()
        {
            CurrentBoostCount++;
            BoostCountChanged?.Invoke();
            Reloaded?.Invoke();
            _isReloading = false;
            CurrentBoostReloadTime = 0;
        }

        private void ApplyBoost()
        {
            CurrentBoostCount--;
            BoostCountChanged?.Invoke();
            _isBoostApplied = true;
            BoostApplied?.Invoke(_boostSpeed);
        }
    }
}