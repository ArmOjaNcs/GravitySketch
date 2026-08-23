using System;
using EnemyScripts.EnemyZones;
using Missile;
using Missile.Configs;
using UnityEngine;

namespace EnemyScripts.Configs
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