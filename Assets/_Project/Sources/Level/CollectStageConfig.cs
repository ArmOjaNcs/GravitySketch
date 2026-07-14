using System.Collections.Generic;
using Assets.Sources.EnemyScripts;
using UnityEngine;

namespace Assets.Sources.Level
{
    [CreateAssetMenu(menuName = "StageConfig/CollectStageConfig")]
    public class CollectStageConfig : ScriptableObject
    {
        public List<AnomalyConfig> AnomalyConfigs;
        public BossConfig BossConfig;
    }
}