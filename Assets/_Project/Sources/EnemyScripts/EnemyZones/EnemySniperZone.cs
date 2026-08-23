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

        public event Action<EnemySniperZone> Deactivated;

        private void OnDisable()
        {
            Deactivated?.Invoke(this);

            if (_cross != null)
                _cross.Shoot -= OnShoot;
        }

        private protected override void Update()
        {
            if (_isCanShoot == false)
                return;

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

            if (sniperConfig != null)
            {
                _cross = Instantiate(sniperConfig.AimCrossPrefab).GetComponent<AimCross>();
                _cross.InitFromConfig(sniperConfig.AimCrossConfig, this);
                _cross.Init(PauseHandler);
                _cross.Shoot += OnShoot;
                Return(_cross.gameObject);
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
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