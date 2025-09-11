using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Table;
using Assets.Sources.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class CollectStagePrefab : MonoBehaviour
    {
        [SerializeField] private List<EnemyPatrolZone> _enemyPatrolZones;
        [SerializeField] private List<SpawnArea> _spawnAreas;
        [SerializeField] private List<MedAid> _medAids;
        [SerializeField] private List<DissolvableObstacle> _dissolvableObstacles;
        [SerializeField] private EnemyPatrolZone _bossPatrolZone;
        [SerializeField] private CollectStageConfig _config;
        [SerializeField] private TemplateColorReference _colorReference;
        [SerializeField] private FenceColorizer _fenceColorizer;

        public List<EnemyPatrolZone> EnemyPatrolZones => _enemyPatrolZones;
        public List<SpawnArea> SpawnAreas => _spawnAreas;
        public List<MedAid> MedAids => _medAids;
        public List<DissolvableObstacle> DissolvableObstacles => _dissolvableObstacles;
        public EnemyPatrolZone BossPatrolZone => _bossPatrolZone;
        public CollectStageConfig Config => _config;
        public TemplateColorReference ColorReference => _colorReference;
        public FenceColorizer FenceColorizer => _fenceColorizer;
    }
}