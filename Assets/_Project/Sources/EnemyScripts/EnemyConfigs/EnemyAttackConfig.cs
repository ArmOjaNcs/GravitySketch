using System;
using EnemyScripts.EnemyZones;
using UnityEngine;

namespace EnemyScripts.Configs
{
    public abstract class EnemyAttackConfig : ScriptableObject
    {
        public float AttackRate;

        public abstract EnemyType Type { get; }
        public abstract Type ZoneComponentType { get; }
    }
}