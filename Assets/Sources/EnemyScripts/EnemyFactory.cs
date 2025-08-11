using Assets.Sources.Audio;
using Assets.Sources.Pause;
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
        [SerializeField] private List<EnemyConfig> _shooterConfigs;
        [SerializeField] private List<EnemyConfig> _sniperConfigs;
        [SerializeField] private List<EnemyConfig> _bomberConfigs;
        [SerializeField] private List<EnemyConfig> _rocketerConfigs;
        [SerializeField] private LayerMask _tableLayer;

        private AudioPlayerSpawner _audioPlayerSpawner;
        private PauseHandler _pauseHandler;

        public int TotalEnemies { get; private set; }

        public void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            _audioPlayerSpawner = audioPlayerSpawner;
            _pauseHandler = pauseHandler;

            foreach (EnemyPatrolZone patrolZone in _patrolZones)
                patrolZone.Initialize();

            TotalEnemies = _patrolZones.Sum(pz => pz.EnemiesCount);
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
                    enemyMover.Init(_pauseHandler);
                    enemy.InitializeFromConfig(config);
                    enemy.Init(_pauseHandler);
                    enemy.SetAudioPlayerSpawner(_audioPlayerSpawner);
                    enemyMover.SetDistance(currentLevel * 5);
                    patrolZone.AddEnemy(enemyMover);

                    if (config.AttackConfig != null)
                    {
                        var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(config.AttackConfig.ZoneComponentType);
                        zone.InitFromConfig(config.AttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
                    }

                    enemy.SetSize(config.Level);
                    config = _bomberConfigs.FirstOrDefault(c => c.Level == currentLevel);
                    enemy.RetreatZone.InitFromConfig(config.AttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);

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