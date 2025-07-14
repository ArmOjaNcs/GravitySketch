using System;
using UnityEngine;
using UnityEngine.AI;
using Assets.Sources.Utils;
using Assets.Sources.Dissolvable;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class Enemy : DissolvableObject
    {
        [SerializeField] private Health _health;
        [SerializeField] private EnemyMover _mover;
        [SerializeField] private GameObject _attackZone;
        [SerializeField] private GameObject _moveZone;
        [SerializeField] private GameObject _stopZone;
        [SerializeField] private EnemyRetreatZone _retreatZone;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Animator[] _fansAnimators;

        private BoxCollider _boxCollider;

        public event Action<bool> Detected;

        public GameObject AttackZone => _attackZone;
        public EnemyRetreatZone RetreatZone => _retreatZone;
        public Transform FirePoint => _firePoint;
        public string Name { get; private set; }
        public bool IsDowned { get; private set; }

        private protected override void Awake()
        {
            base.Awake();
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
            ApplyRandomColors();
        }

        private void OnEnable()
        {
            if (_health.CurrentValue > 0)
                _mover.Activate();

            ActivateFans();
        }

        private protected override void OnDisable()
        {
            if (_health.CurrentValue > 0)
                _mover.Deactivate();

            base.OnDisable();
        }

        public override void Pause()
        {
            base.Pause();
            StopFans();
        }

        public override void Resume()
        {
            base.Resume();

            if (IsDowned == false)
                ActivateFans();
        }

        public override void DropDown()
        {
            base.DropDown();

            IsDowned = true;
            _mover.Deactivate();
            _boxCollider.isTrigger = false;
            StopFans();
        }

        public void TakeDamage(float damage = 1)
        {
            _health.TakeDamage(damage);

            if (_health.CurrentValue <= 0)
                DropDown();
        }

        public void Detect(bool isDetected) => Detected?.Invoke(isDetected);

        public void InitializeFromConfig(EnemyConfig config)
        {
            _health.Initialize(config.Health);
            transform.localScale = config.Scale;
            Name = config.Name;
            SetZonesScale(config);

            var agent = GetComponent<NavMeshAgent>();
            agent.speed = config.Speed;
            agent.acceleration = config.Acceleration;
            _mover.Activate();
        }

        private void ApplyRandomColors()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);

            foreach (MeshRenderer renderer in renderers)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(UserUtils.ColorID, UserUtils.GetRandomColor());
                renderer.SetPropertyBlock(block);
            }
        }

        private void SetZonesScale(EnemyConfig enemyConfig)
        {
            _attackZone.transform.localScale = enemyConfig.AttackZoneScale;
            _moveZone.transform.localScale = enemyConfig.MoveZoneScale;
            _stopZone.transform.localScale = enemyConfig.StopZoneScale;
            _retreatZone.transform.localScale = enemyConfig.RetreatZoneScale;
        }

        private void StopFans()
        {
            foreach(Animator animator in _fansAnimators)
                animator.enabled = false;
        }

        private void ActivateFans()
        {
            foreach (Animator animator in _fansAnimators)
                animator.enabled = true;
        }
    }
}