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
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private Player _player;
        [SerializeField] private HoleMaskHandler _maskHandler;
        [SerializeField] private CollectStage _collectStage;

        private protected override void OnLoadWindowUpdated()
        {
            base.OnLoadWindowUpdated();
            _playerInput.StartInput();
        }

        private protected override void Initialize()
        {
            _maskHandler.Init(PauseHandler);
            _playerInput.Init(PauseHandler);
            _enemyFactory.Init(AudioPlayerSpawner, PauseHandler);
            _collectStage.Init(PauseHandler);

            foreach (DissolvableObstacle obstacle in _obstacles)
            {
                obstacle.SetAudioPlayerSpawner(AudioPlayerSpawner);
                obstacle.Init(PauseHandler);
            }

            foreach (PauseableObject pauseable in Objects)
                pauseable.Init(PauseHandler);

            _simpleCubeSpawner.Init(PauseHandler, AudioPlayerSpawner);
            _player.Init(PauseHandler);
        }
    }
}