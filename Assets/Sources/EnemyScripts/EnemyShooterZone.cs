using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyShooterZone : EnemyAttackZone
    {
        private ObjectPool<Bullet> _pool;
        private BulletConfig _config;

        public override void Initialize(EnemyAttackConfig config, Transform firePoint)
        {
            base.Initialize(config, firePoint);

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
            if (Player == null)
                return;

            CurrentTime = 0;
            Bullet bullet = _pool.GetElement();

            if (bullet.IsInitialized == false)
                bullet.Initialize(_config, this);

            bullet.transform.position = FirePoint.position;
            bullet.gameObject.SetActive(true);
            bullet.Send(FirePoint.position, Player.Position);
        }
    }
}