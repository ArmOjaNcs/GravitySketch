using System;
using UnityEngine;
using Assets.Sources.Pause;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Shield : PauseableObject
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField, Min(0)] private float _defendTime;
        [SerializeField, Min(0)] private float _reloadTime;
        [SerializeField, Min(0)] private float _defendUpgradeDelta;
        [SerializeField, Min(0)] private float _reloadUpgradeDelta;
        [SerializeField, Min(1)] private float _maxDefendTime;

        private MeshRenderer _meshRenderer;
        private bool _isDefended;
        private bool _isDefendApplied;

        public event Action DefendApplied;
        public event Action Reloading;

        public float CurrentDefendTime { get; private set; }
        public float CurrentReloadTime { get; private set; }
        public float CycleTime { get; private set; }
        public float DefendTime => _defendTime;
        public bool IsDefended => _isDefended;
        public bool IsReloading { get; private set; }

        private protected override void Awake()
        {
            base.Awake();
            CycleTime = _reloadTime + _defendTime;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
        }

        private void OnEnable()
        {
            _playerInput.Defended += OnDefended;
        }

        private void OnDisable()
        {
            _playerInput.Defended -= OnDefended;
        }

        private void Update()
        {
            if(IsPaused) 
                return;

            if (_isDefendApplied)
                PlayCycle();
        }

        public void UpgradeShield()
        {
            _defendTime += _defendUpgradeDelta;
            CycleTime -= _reloadUpgradeDelta;
            _defendTime = Mathf.Clamp(_defendTime, 0, _maxDefendTime);
            CycleTime = Mathf.Clamp(CycleTime, _defendTime + 1, float.MaxValue);
        }

        private void OnDefended()
        {
            if (_isDefendApplied)
                return;

            _isDefendApplied = true;
            _isDefended = true;
            _meshRenderer.enabled = true;
            DefendApplied?.Invoke();
        }

        private void PlayCycle()
        {
            CurrentDefendTime += Time.deltaTime;

            if (CurrentDefendTime > _defendTime && IsReloading == false)
            {
                _isDefended = false;
                _meshRenderer.enabled = false;
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
                    CurrentDefendTime = 0;
                }
            }
        }
    }
}