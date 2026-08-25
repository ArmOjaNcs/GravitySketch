using System;
using Audio;
using EnemyScripts.Configs;
using Missile;
using Pause;
using Utils;
using UnityEngine;

namespace EnemyScripts.EnemyZones
{
    public class EnemySniperZone : EnemyAttackZone
    {
        private AimCross _cross;
        private bool _isCanShoot = true;
        private bool _isDeactivated;
        private Enemy _enemy;

        public event Action<EnemySniperZone> Deactivated;

        private protected override void Awake()
        {
            base.Awake();
            _enemy = GetComponentInParent<Enemy>();
        }

        private void OnDisable()
        {
            if (_cross != null)
                _cross.Shoot -= OnShoot;
        }

        private protected override void Update()
        {
            if (IsInitialized == false)
                return;

            if (_isCanShoot == false)
                return;

            if(_enemy.IsDowned && _isDeactivated == false)
            {
                Deactivated?.Invoke(this);
                _isDeactivated = true;
            }

            base.Update();
        }

        public override void InitFromConfig(
            EnemyAttackConfig config,
            Transform firePoint,
            AudioPlayerSpawner audioPlayerSpawner,
            PauseHandler pauseHandler)
        {
            base.InitFromConfig(config, firePoint, audioPlayerSpawner, pauseHandler);

            SniperConfig sniperConfig = config.SafeCast<SniperConfig>();

            if (sniperConfig == null)
                return;

            _cross = Instantiate(sniperConfig.AimCrossPrefab).GetComponent<AimCross>();
            _cross.InitFromConfig(sniperConfig.AimCrossConfig, this);
            _cross.Init(PauseHandler);
            _cross.Shoot += OnShoot;
            Return(_cross.gameObject);
            IsInitialized = true;
        }

        public override void Return(GameObject gameObject)
        {
            _isCanShoot = true;
            base.Return(gameObject);
        }

        private protected override void Attack()
        {
            base.Attack();

            _isCanShoot = false;
            _cross.gameObject.SetActive(true);
            _cross.StartAimWarning();
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/AimCross/SniperShot");
        }

        private void OnShoot()
        {
            AudioPlayer?.Play();
        }
    }
}