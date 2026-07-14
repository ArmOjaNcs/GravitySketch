using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [CreateAssetMenu(fileName = "ShooterConfig", menuName = "Enemy/Attack/Shooter")]
    public class ShooterConfig : EnemyAttackConfig
    {
        public Bullet BulletPrefab;
        public int Capacity;
        public BulletConfig BulletConfig;

        public override EnemyType Type => EnemyType.Shooter;

        public override System.Type ZoneComponentType => typeof(EnemyShooterZone);
    }
}