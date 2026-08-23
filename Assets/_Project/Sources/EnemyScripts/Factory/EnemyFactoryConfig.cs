using System.Collections.Generic;
using EnemyScripts.Configs;
using UnityEngine;

namespace EnemyScripts.Factory
{
    [CreateAssetMenu(menuName = "StageConfig/EnemyFactoryConfig")]
    public class EnemyFactoryConfig : ScriptableObject
    {
        public List<EnemyConfig> ShooterConfigs;
        public List<EnemyConfig> SniperConfigs;
        public List<EnemyConfig> BomberConfigs;
        public List<EnemyConfig> RocketerConfigs;
    }
}