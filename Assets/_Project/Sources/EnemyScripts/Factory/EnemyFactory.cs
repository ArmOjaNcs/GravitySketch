using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using EnemyScripts.Configs;
using EnemyScripts.EnemyZones;
using Pause;
using UnityEngine;

namespace EnemyScripts.Factory
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private LayerMask _tableLayer;

        private EnemyPatrolZone _bossPatrolZone;
        private List<EnemyPatrolZone> _patrolZones = new ();
        private EnemyFactoryConfig _config;
        private BossConfig _bossConfig;
        private AudioPlayerSpawner _audioPlayerSpawner;
        private PauseHandler _pauseHandler;
        private Queue<Enemy> _enemiesQueue = new ();

        public event Action EnemiesSpawned;

        public int TotalEnemies { get; private set; }

        public bool IsBossSpawned { get; private set; }

        public void Init(
            PauseHandler pauseHandler,
            AudioPlayerSpawner audioPlayerSpawner,
            EnemyFactoryConfig enemyFactoryConfig,
            BossConfig bossConfig,
            List<EnemyPatrolZone> enemyPatrolZones,
            EnemyPatrolZone bossPatrolZone)
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
            TotalEnemies++;

            for (int i = 0; i < TotalEnemies; i++)
                CreateNewEnemyInQueue();

            CreateEnemies();
        }

        public Enemy CreateBoss()
        {
            Enemy enemy = SpawnEnemyInZone(_bossPatrolZone, _bossConfig.Config, true);
            enemy.Mover.SetIsBoss();

            foreach (EnemyAttackConfig enemyAttackConfig in _bossConfig.AttackConfigs)
            {
                var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(enemyAttackConfig.ZoneComponentType);
                zone.InitFromConfig(enemyAttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
            }

            enemy.SetSize(_bossConfig.Config.Level);
            var config = _bossConfig.Config.AttackConfig;
            enemy.RetreatZone.InitFromConfig(config, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
            enemy.gameObject.SetActive(true);
            IsBossSpawned = true;
            return enemy;
        }

        private void CreateNewEnemyInQueue()
        {
            Enemy enemy = Instantiate(_enemy);
            enemy.Dissolved += OnEnemyDissolved;
            enemy.gameObject.SetActive(false);
            _enemiesQueue.Enqueue(enemy);
        }

        private void CreateEnemies()
        {
            foreach (EnemyPatrolZone patrolZone in _patrolZones)
            {
                for (int i = 0; i < Mathf.CeilToInt(patrolZone.EnemiesCount / 2f); i++)
                {
                    EnemyConfig config = GetEnemyConfig(patrolZone);
                    SpawnEnemyInZone(patrolZone, config);
                }
            }

            EnemiesSpawned?.Invoke();
        }

        private Enemy SpawnEnemyInZone(EnemyPatrolZone patrolZone, EnemyConfig config, bool isBoss = false)
        {
            if (_enemiesQueue.Count == 0)
                CreateNewEnemyInQueue();

            Enemy enemy = _enemiesQueue.Dequeue();
            TryGetFreePosition(10, patrolZone, out Vector3 freePosition);
            enemy.transform.position = freePosition;
            enemy.Mover.SetPatrolZone(patrolZone);
            enemy.Mover.Init(_pauseHandler);
            enemy.InitializeFromConfig(config);
            enemy.Init(_pauseHandler);
            enemy.SetAudioPlayerSpawner(_audioPlayerSpawner);
            enemy.Mover.CalculateRetreatDistance();
            enemy.Mover.SetPatrolDistance(config.Level * 5);

            if (isBoss == false)
            {
                patrolZone.AddEnemy();

                if (patrolZone.IsPlayerIn)
                    enemy.Mover.OnPlayerInZone();

                if (config.AttackConfig != null)
                {
                    var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(config.AttackConfig.ZoneComponentType);
                    zone.InitFromConfig(config.AttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);
                }

                enemy.SetSize(config.Level);
                config = _config.BomberConfigs.FirstOrDefault(c => c.Level == enemy.Size);
                enemy.RetreatZone.InitFromConfig(
                    config.AttackConfig, enemy.FirePoint, _audioPlayerSpawner, _pauseHandler);

                enemy.gameObject.SetActive(true);
            }

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

        private void OnEnemyDissolved(Enemy enemy)
        {
            enemy.Dissolved -= OnEnemyDissolved;

            if (enemy.Mover.IsBoss)
                return;

            if (enemy.Mover.PatrolZone.CurrentEnemiesCount >= enemy.Mover.PatrolZone.EnemiesCount)
                return;

            EnemyConfig enemyConfig = GetEnemyConfig(enemy.Mover.PatrolZone);
            SpawnEnemyInZone(enemy.Mover.PatrolZone, enemyConfig);
        }

        private EnemyConfig GetEnemyConfig(EnemyPatrolZone patrolZone)
        {
            int created = patrolZone.CurrentEnemiesCount;
            int configIndex = 0;
            int minLevel = patrolZone.MinLevel;
            int maxLevel = patrolZone.MaxLevel;
            int currentLevel = minLevel;
            configIndex = created % 4;
            currentLevel += created / 4;
            currentLevel = (currentLevel > maxLevel) ? minLevel : currentLevel;
            EnemyConfig config = null;

            switch (configIndex)
            {
                case 0:
                    return config = _config.ShooterConfigs.FirstOrDefault(c => c.Level == currentLevel);

                case 1:
                    return config = _config.SniperConfigs.FirstOrDefault(c => c.Level == currentLevel);

                case 2:
                    return config = _config.BomberConfigs.FirstOrDefault(c => c.Level == currentLevel);

                case 3:
                    return config = _config.RocketerConfigs.FirstOrDefault(c => c.Level == currentLevel);

                default: return config;
            }
        }
    }
}