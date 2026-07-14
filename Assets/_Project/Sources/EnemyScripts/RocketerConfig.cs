using System;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [CreateAssetMenu(fileName = "RocketerConfig", menuName = "Enemy/Attack/Rocketer")]

    public class RocketerConfig : EnemyAttackConfig
    {
        public Rocket RocketPrefab;
        public RocketConfig RocketConfig;
        public int Capacity;

        public override EnemyType Type => EnemyType.Rocketer;

        public override Type ZoneComponentType => typeof(EnemyRocketZone);
    }
}