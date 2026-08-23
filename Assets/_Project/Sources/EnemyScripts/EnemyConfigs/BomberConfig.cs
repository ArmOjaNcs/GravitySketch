using System;
using EnemyScripts.EnemyZones;
using Missile;
using Missile.Configs;
using UnityEngine;

namespace EnemyScripts.Configs
{
    [CreateAssetMenu(fileName = "BomberConfig", menuName = "Enemy/Attack/Bomber")]
    public class BomberConfig : EnemyAttackConfig
    {
        public ThrowableBomb BombPrefab;
        public BombConfig BombConfig;
        public int Capacity;

        public override EnemyType Type => EnemyType.Bomber;

        public override Type ZoneComponentType => typeof(EnemyBomberZone);
    }
}