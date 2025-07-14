using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public abstract class EnemyAttackConfig : ScriptableObject
    {
        public float AttackRate;
        public abstract EnemyType Type { get; }
        public abstract System.Type ZoneComponentType { get; }
    }
}