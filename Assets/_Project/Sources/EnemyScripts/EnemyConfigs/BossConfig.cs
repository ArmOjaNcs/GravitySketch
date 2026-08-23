using System.Collections.Generic;
using UnityEngine;

namespace EnemyScripts.Configs
{
    [CreateAssetMenu(menuName = "StageConfig/BossConfig")]
    public class BossConfig : ScriptableObject
    {
        public EnemyConfig Config;
        public List<EnemyAttackConfig> AttackConfigs;
    }
}