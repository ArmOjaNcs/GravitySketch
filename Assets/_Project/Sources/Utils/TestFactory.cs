using Assets.Sources.Audio;
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
        [SerializeField] private AudioPlayerSpawner _audioPlayerSpawner;
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
        }
    }
}