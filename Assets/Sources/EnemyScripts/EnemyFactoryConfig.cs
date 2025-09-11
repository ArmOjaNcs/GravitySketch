using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [CreateAssetMenu(menuName = "StageConfig/EnemyFactoryConfig")]
    public class EnemyFactoryConfig : ScriptableObject
    {
        public List<EnemyConfig> ShooterConfigs;
        public List<EnemyConfig> SniperConfigs;
        public List<EnemyConfig> BomberConfigs;
        public List<EnemyConfig> RocketerConfigs;
        public EnemyConfig BossConfig;
        public List<EnemyAttackConfig> BossAttackConfigs;
    }
}