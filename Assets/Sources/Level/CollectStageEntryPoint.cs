using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Table;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class CollectStageEntryPoint : StageEntryPoint
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private List<DissolvableObstacle> _obstacles;
        [SerializeField] private List<DissolvableObject> _dissolvableObjects;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private HoleMaskHandler _maskHandler;
        [SerializeField] private GrowHandler _growHandler;

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
            _maskHandler.Init(PauseHandler);
            _playerInput.Init(PauseHandler);
            _enemyFactory.Init(PauseHandler, AudioPlayerSpawner);

            foreach (DissolvableObstacle obstacle in _obstacles)
            {
                obstacle.SetAudioPlayerSpawner(AudioPlayerSpawner);
                obstacle.Init(PauseHandler);
            }

            foreach (DissolvableObject @object in _dissolvableObjects)
            {
                @object.SetAudioPlayerSpawner(AudioPlayerSpawner);
                @object.Init(PauseHandler);
            }

            foreach (PauseableObject pauseable in Objects)
                pauseable.Init(PauseHandler);

            Stage.Init(PauseHandler, AudioPlayerSpawner);
            _simpleCubeSpawner.Init(PauseHandler, AudioPlayerSpawner);
        }

        private protected override void OnLoadWindowUpdated()
        {
            base.OnLoadWindowUpdated();
            _playerInput.StartInput();
        }
    }
}