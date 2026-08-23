using Audio;
using EnemyScripts.Configs;
using Missile;
using Missile.Configs;
using Pause;
using Utils;
using UnityEngine;

namespace EnemyScripts.EnemyZones
{
    public class EnemyBomberZone : EnemyAttackZone
    {
        private ObjectPool<ThrowableBomb> _pool;
        private BombConfig _config;

        public override void InitFromConfig(
            EnemyAttackConfig config,
            Transform firePoint,
            AudioPlayerSpawner audioPlayerSpawner,
            PauseHandler pauseHandler)
        {
            base.InitFromConfig(config, firePoint, audioPlayerSpawner, pauseHandler);

            BomberConfig bomberConfig = config.SafeCast<BomberConfig>();

            if (bomberConfig != null)
            {
                _pool = new ObjectPool<ThrowableBomb>(bomberConfig.BombPrefab, bomberConfig.Capacity, transform);
                _config = bomberConfig.BombConfig;
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
        }

        private protected override void Attack()
        {
            base.Attack();

            ThrowableBomb bomb = _pool.GetElement();

            if (bomb.IsInitialized == false)
            {
                bomb.InitFromConfig(_config, this);
                bomb.Init(PauseHandler);
            }

            bomb.transform.position = FirePoint.position;
            bomb.gameObject.SetActive(true);
            AudioPlayer?.Play();
            bomb.AddForces(FirePoint.position);
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/Bomb/GrenadeLauncher");
        }
    }
}