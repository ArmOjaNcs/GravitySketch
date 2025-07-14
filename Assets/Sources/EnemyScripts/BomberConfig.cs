using System;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
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