using EnemyScripts.EnemyZones;
using Missile;
using Missile.Configs;
using UnityEngine;

namespace EnemyScripts.Configs
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