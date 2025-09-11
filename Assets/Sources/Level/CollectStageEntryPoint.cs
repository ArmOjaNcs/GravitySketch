using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Table;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class CollectStageEntryPoint : StageEntryPoint
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private AnomalySpawner _anomalySpawner;
        [SerializeField] private List<DissolvableObstacle> _obstacles;
        [SerializeField] private List<DissolvableObject> _dissolvableObjects;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private HoleMaskHandler _maskHandler;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Player _player;

        private CollectStagePrefab _collectStagePrefab;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
        }

        private void OnGrowing()
        {
            foreach (DissolvableObstacle obstacle in _obstacles)
            {
                if (obstacle.Size < _growHandler.CurrentSize)
                    obstacle.DropDown();
            }
        }

        private protected override void Initialize()
        {
            GameObject prefab = Resources.Load<GameObject>(Stage.StageName);
            prefab = Instantiate(prefab);
            _collectStagePrefab = prefab.GetComponent<CollectStagePrefab>();
            _maskHandler.Init(PauseHandler);
            _playerInput.Init(PauseHandler);
            _enemyFactory.Init(PauseHandler, AudioPlayerSpawner, _collectStagePrefab.Config.EnemyFactoryConfig, 
                _collectStagePrefab.EnemyPatrolZones, _collectStagePrefab.BossPatrolZone);
            _anomalySpawner.Init(PauseHandler, AudioPlayerSpawner, _collectStagePrefab.Config.AnomalyConfigs);
            _simpleCubeSpawner.Init(PauseHandler, AudioPlayerSpawner, _collectStagePrefab.SpawnAreas,
               _collectStagePrefab.ColorReference);

            foreach (DissolvableObstacle obstacle in _collectStagePrefab.DissolvableObstacles)
            {
                obstacle.SetAudioPlayerSpawner(AudioPlayerSpawner);
                obstacle.Init(PauseHandler);
            }

            foreach (MedAid medAid in _collectStagePrefab.MedAids)
            {
                medAid.SetAudioPlayerSpawner(AudioPlayerSpawner);
                medAid.Init(PauseHandler);
            }

            foreach (PauseableObject pauseable in Objects)
                pauseable.Init(PauseHandler);

            Stage.Init(PauseHandler, AudioPlayerSpawner);
            _player.transform.position = _collectStagePrefab.Config.PlayerStartPosition;
            _collectStagePrefab.FenceColorizer.ColorizeFence(_collectStagePrefab.ColorReference);
        }

        private protected override void OnLoadWindowUpdated()
        {
            base.OnLoadWindowUpdated();
            _playerInput.StartInput();
        }
    }
}