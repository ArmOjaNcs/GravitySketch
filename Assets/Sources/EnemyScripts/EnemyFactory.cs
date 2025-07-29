using Assets.Sources.PlayerScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyFactory : MonoBehaviour
    {
        [Header("PatrolZone")]
        [SerializeField] private List<EnemyPatrolZone> _patrolZones;

        [Header("Enemy settings")]
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Player _player;
        [SerializeField] private List<EnemyConfig> _shooterConfigs;
        [SerializeField] private List<EnemyConfig> _sniperConfigs;
        [SerializeField] private List<EnemyConfig> _bomberConfigs;
        [SerializeField] private List<EnemyConfig> _rocketerConfigs;
        [SerializeField] private LayerMask _tableLayer;

        public int TotalEnemies { get; private set; }

        private void Awake()
        {
            foreach (EnemyPatrolZone patrolZone in _patrolZones)
                patrolZone.Initialize();

            TotalEnemies = _patrolZones.Sum(pz => pz.EnemiesCount);
        }

        private void Start()
        {
            CreateEnemy();
        }

        private void CreateEnemy()
        {
            foreach (EnemyPatrolZone patrolZone in _patrolZones)
            {
                int created = 0;
                int configIndex = 0;
                int minLevel = patrolZone.MinLevel;
                int maxLevel = patrolZone.MaxLevel;
                int currentLevel = minLevel;
                EnemyConfig config = null;

                for (int i = 0; i < patrolZone.EnemiesCount; i++)
                {
                    configIndex = created % 4;
                    currentLevel += created / 4;
                    currentLevel = (currentLevel > maxLevel) ? minLevel : currentLevel;

                    switch (configIndex)
                    {
                        case 0:
                            config = _shooterConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;

                        case 1:
                            config = _sniperConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;

                        case 2:
                            config = _bomberConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;

                        case 3:
                            config = _rocketerConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;
                    }

                    TryGetFreePosition(10, patrolZone, out Vector3 freePosition);
                    Enemy enemy = Instantiate(_enemy, freePosition, Quaternion.identity);
                    EnemyMover enemyMover = enemy.GetComponent<EnemyMover>();
                    enemyMover.SetMovePointsHolder(patrolZone.MovePointsHolder);
                    enemyMover.SetDistance(currentLevel * 5);
                    patrolZone.AddEnemy(enemyMover);

                    enemy.InitializeFromConfig(config);

                    if (config.AttackConfig != null)
                    {
                        var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(config.AttackConfig.ZoneComponentType);
                        zone.Initialize(config.AttackConfig, enemy.FirePoint);
                    }

                    enemy.Init(config.Level);
                    config = _bomberConfigs.FirstOrDefault(c => c.Level == currentLevel);
                    enemy.RetreatZone.Initialize(config.AttackConfig, enemy.FirePoint);

                    created++;
                }
            }
        }

        private bool IsSpawnAreaFree(Vector3 position)
        {
            return Physics.OverlapSphere(position, 1, _tableLayer).Length == 0;
        }

        private bool TryGetFreePosition(int attempsCount, EnemyPatrolZone patrolZone, out Vector3 freePosition)
        {
            for (int i = 0; i < attempsCount; i++)
            {
                Vector3 position = patrolZone.GetRandomPointInZone();

                if (IsSpawnAreaFree(position))
                {
                    freePosition = position;
                    return true;
                }
            }

            freePosition = Vector3.zero;
            return false;
        }
    }
}