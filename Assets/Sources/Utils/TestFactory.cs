using Assets.Sources.EnemyScripts;
using UnityEngine;

namespace Assets.Sources.Utils
{
    public class TestFactory : MonoBehaviour
    {
        [Header("PatrolZone")]
        [SerializeField] private EnemyPatrolZone _patrolZone;

        [Header("Enemy settings")]
        [SerializeField] private Enemy _enemy;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private BomberConfig _bomberConfig;
        [SerializeField] private Transform _spawnPoint;

        private void Awake()
        {
            _patrolZone.Initialize();
        }

        private void Start()
        {
            CreateEnemy();
        }

        private void CreateEnemy()
        {
            Enemy enemy = Instantiate(_enemy, _spawnPoint.position, Quaternion.identity);
            EnemyMover enemyMover = enemy.GetComponent<EnemyMover>();
            enemyMover.SetMovePointsHolder(_patrolZone.MovePointsHolder);
            enemyMover.SetDistance(_enemyConfig.Level * 5);
            _patrolZone.AddEnemy(enemyMover);

            enemy.InitializeFromConfig(_enemyConfig);

            if (_enemyConfig.AttackConfig != null)
            {
                var zone = (IEnemyAttack)enemy.AttackZone.AddComponent(_enemyConfig.AttackConfig.ZoneComponentType);
                zone.Initialize(_enemyConfig.AttackConfig, enemy.FirePoint);
            }

            enemy.Init(_enemyConfig.Level);
            enemy.RetreatZone.Initialize(_bomberConfig, enemy.FirePoint);
        }
    }
}