using Assets.Sources.EnemyScripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    [CreateAssetMenu(menuName = "StageConfig/CollectStageConfig")]
    public class CollectStageConfig : ScriptableObject
    {
        public List<AnomalyConfig> AnomalyConfigs;
        public EnemyFactoryConfig EnemyFactoryConfig;
        public Vector3 PlayerStartPosition;
    }
}