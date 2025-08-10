using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyShooterZone : EnemyAttackZone
    {
        private ObjectPool<Bullet> _pool;
        private BulletConfig _config;

        public override void InitFromConfig(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner, PauseHandler pauseHandler)
        {
            base.InitFromConfig(config, firePoint, audioPlayerSpawner, pauseHandler);

            ShooterConfig shooterConfig = config.SafeCast<ShooterConfig>();

            if (shooterConfig != null)
            {
                _pool = new ObjectPool<Bullet>(shooterConfig.BulletPrefab, shooterConfig.Capacity, transform);
                _config = shooterConfig.BulletConfig;
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
        }

        private protected override void Attack()
        {
            base.Attack();
            Bullet bullet = _pool.GetElement();

            if (bullet.IsInitialized == false)
            {
                bullet.InitFromConfig(_config, this);
                bullet.Init(PauseHandler);
            }

            bullet.transform.position = FirePoint.position;
            bullet.gameObject.SetActive(true);
            AudioPlayer.Play();
            bullet.Send(FirePoint.position, Player.Position);
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/Shooter/ShooterShot");
        }
    }
}