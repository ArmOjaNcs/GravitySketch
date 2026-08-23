using EnemyScripts.EnemyZones;
using Missile;
using Missile.Configs;
using UnityEngine;

namespace EnemyScripts.Configs
{
    [CreateAssetMenu(fileName = "SniperConfig", menuName = "Enemy/Attack/Sniper")]
    public class SniperConfig : EnemyAttackConfig
    {
        public AimCrossConfig AimCrossConfig;
        public GameObject AimCrossPrefab;

        public override EnemyType Type => EnemyType.Sniper;

        public override System.Type ZoneComponentType => typeof(EnemySniperZone);
    }
}