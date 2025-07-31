using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Shield : PlayerAbility
    {
        [SerializeField, Min(0)] private float _defendUpgradeDelta;
        [SerializeField, Min(1)] private float _maxDefendTime;

        private MeshRenderer _meshRenderer;
        private bool _isDefended;
        private bool _isDefendApplied;

        public event Action DefendApplied;
        public event Action Reloading;

        public float CycleTime { get; private set; }
        public float DefendTime => ActiveTime;
        public bool IsDefended => _isDefended;
        public bool IsReloading { get; private set; }

        private protected override void Awake()
        {
            base.Awake();
            CycleTime = ReloadTime + ActiveTime;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
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
            if(IsPaused) 
                return;

            if (_isDefendApplied)
                PlayCycle();
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
            CurrentActiveTime += Time.deltaTime;

            if (CurrentActiveTime > ActiveTime && IsReloading == false)
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
                    CurrentActiveTime = 0;
                }
            }
        }

        public override void Upgrade()
        {
            ActiveTime += _defendUpgradeDelta;
            CycleTime -= ReloadUpgradeDelta;
            ActiveTime = Mathf.Clamp(ActiveTime, 0, _maxDefendTime);
            CycleTime = Mathf.Clamp(CycleTime, ActiveTime + 1, float.MaxValue);
        }
    }
}