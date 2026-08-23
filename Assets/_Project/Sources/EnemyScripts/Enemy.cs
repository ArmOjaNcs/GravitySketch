using System;
using Dissolvable;
using EnemyScripts.Configs;
using EnemyScripts.EnemyZones;
using Pause;
using Utils;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
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
        [SerializeField] private FansAnimator _fansAnimator;
        [SerializeField] private PauseableObject[] _pauseableObjects;

        public event Action<bool> Detected;
        public event Action Downed;
        public event Action<Enemy> Dissolved;

        public GameObject AttackZone => _attackZone;
        public EnemyRetreatZone RetreatZone => _retreatZone;
        public Transform FirePoint => _firePoint;
        public EnemyMover Mover => _mover;
        public string Name { get; private set; }
        public bool IsDowned { get; private set; }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            ApplyRandomColors();

            foreach (PauseableObject pauseableObject in _pauseableObjects)
                pauseableObject.Init(pauseHandler);

            ActivateFans();
            IsInitialized = true;
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
            Downed?.Invoke();
            _mover.Deactivate();
            Collider.isTrigger = false;
            StopFans();
        }

        public void TakeDamage(float damage = 1)
        {
            if (IsDowned)
                return;

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
            SetSize(config.Level);

            var agent = GetComponent<NavMeshAgent>();
            agent.speed = config.Speed;
            agent.acceleration = config.Acceleration;
            _mover.Activate();
        }

        private protected override void OnRoutineEnd()
        {
            Dissolved?.Invoke(this);
            base.OnRoutineEnd();
        }

        private void ApplyRandomColors()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            foreach (MeshRenderer renderer in renderers)
            {
                int materialCount = renderer.sharedMaterials.Length;

                for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
                {
                    mpb.Clear();
                    mpb.SetColor(UserUtils.ColorID, UserUtils.GetRandomColor());
                    renderer.SetPropertyBlock(mpb, materialIndex);
                }
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
            _fansAnimator.Deactivate();
        }

        private void ActivateFans()
        {
            _fansAnimator.Activate();
        }
    }
}