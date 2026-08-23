using System.Collections.Generic;
using AnomalyScripts;
using EnemyScripts.Configs;
using UnityEngine;

namespace Level.StageScripts
{
    [CreateAssetMenu(menuName = "StageConfig/CollectStageConfig")]
    public class CollectStageConfig : ScriptableObject
    {
        public List<AnomalyConfig> AnomalyConfigs;
        public BossConfig BossConfig;
    }
}