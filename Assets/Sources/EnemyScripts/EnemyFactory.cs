using Assets.Sources.Audio;
using Assets.Sources.Pause;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private LayerMask _tableLayer;

        private EnemyPatrolZone _bossPatrolZone;
        private List<EnemyPatrolZone> _patrolZones = new();
        private EnemyFactoryConfig _config;
        private BossConfig _bossConfig;
        private AudioPlayerSpawner _audioPlayerSpawner;
        private PauseHandler _pauseHandler;

        public int TotalEnemies { get; private set; }
        public bool IsBossSpawned { get; private set; }

        public void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner, 
            EnemyFactoryConfig enemyFactoryConfig, BossConfig bossConfig, List<EnemyPatrolZone> enemyPatrolZones, EnemyPatrolZone bossPatrolZone)
        {
            _audioPlayerSpawner = audioPlayerSpawner;
            _pauseHandler = pauseHandler;
            _config = enemyFactoryConfig;
            _bossConfig = bossConfig;
            _patrolZones = enemyPatrolZones;
            _bossPatrolZone = bossPatrolZone;

            foreach (EnemyPatrolZone patrolZone in _patrolZones)
                patrolZone.Initialize();

            TotalEnemies = _patrolZones.Sum(pz => pz.EnemiesCount);
            CreateEnemy();
        }

        public Enemy CreateBoss()
        {
            Enemy enemy = SpawnEnemyInZone(_bossPatrolZone, _bossConfig.Config);

            foreach (EnemyAttackConfig enemyAttackConfig in _bossConfig.AttackConfigs)
            {
                var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(enemyAttackConfig.ZoneComponentType);
                zone.InitFromConfig(enemyAttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
            }

            enemy.SetSize(_bossConfig.Config.Level);
            var config = _bossConfig.Config.AttackConfig;
            enemy.RetreatZone.InitFromConfig(config, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
            IsBossSpawned = true;
            return enemy;
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
                            config = _config.ShooterConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;

                        case 1:
                            config = _config.SniperConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;

                        case 2:
                            config = _config.BomberConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;

                        case 3:
                            config = _config.RocketerConfigs.FirstOrDefault(c => c.Level == currentLevel);
                            break;
                    }

                    Enemy enemy = SpawnEnemyInZone(patrolZone, config);

                    if (config.AttackConfig != null)
                    {
                        var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(config.AttackConfig.ZoneComponentType);
                        zone.InitFromConfig(config.AttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
                    }

                    enemy.SetSize(config.Level);
                    config = _config.BomberConfigs.FirstOrDefault(c => c.Level == currentLevel);
                    enemy.RetreatZone.InitFromConfig(config.AttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);

                    created++;
                }
            }
        }

        private Enemy SpawnEnemyInZone(EnemyPatrolZone patrolZone, EnemyConfig config)
        {
            TryGetFreePosition(10, patrolZone, out Vector3 freePosition);
            Enemy enemy = Instantiate(_enemy, freePosition, Quaternion.identity);
            EnemyMover enemyMover = enemy.GetComponent<EnemyMover>();
            enemyMover.SetPatrolZone(patrolZone);
            enemyMover.Init(_pauseHandler);
            enemy.InitializeFromConfig(config);
            enemy.Init(_pauseHandler);
            enemy.SetAudioPlayerSpawner(_audioPlayerSpawner);
            enemyMover.CalculateRetreatDistance();
            enemyMover.SetPatrolDistance(config.Level * 5);
            patrolZone.AddEnemy(enemyMover);
            return enemy;
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