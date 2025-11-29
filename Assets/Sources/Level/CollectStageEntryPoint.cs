using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Table;
using Assets.Sources.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Sources.Level
{
    public class CollectStageEntryPoint : StageEntryPoint
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private AnomalySpawner _anomalySpawner;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private HoleMaskHandler _maskHandler;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Player _player;
        [SerializeField] private EnemyFactoryConfig _factoryConfig;

        private CollectStagePrefab _collectStagePrefab;
        private NavMeshDataInstance _navMeshInstance;

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
            foreach (DissolvableObstacle obstacle in _collectStagePrefab.DissolvableObstacles)
            {
                if (obstacle.Size < _growHandler.CurrentSize)
                    obstacle.DropDown();
            }
        }

        private protected override void Initialize()
        {
            if(Stage.IsTutorial)
                Prefab = Resources.Load<GameObject>(UserUtils.TutorialCollectName);
            else
                Prefab  = Resources.Load<GameObject>(Stage.StageName);

            Prefab = Instantiate(Prefab);
            _collectStagePrefab = Prefab.GetComponent<CollectStagePrefab>();
            _maskHandler.Init(PauseHandler, _collectStagePrefab.Renderer, _collectStagePrefab.TableMaterial);
            _playerInput.Init(PauseHandler);
            _anomalySpawner.Init(PauseHandler, AudioPlayerSpawner, _collectStagePrefab.Config.AnomalyConfigs);
            _simpleCubeSpawner.Init(PauseHandler, AudioPlayerSpawner, _collectStagePrefab.SpawnAreas,
               _collectStagePrefab.ColorReference);

            if (_collectStagePrefab.NavMeshData != null)
            {
                _navMeshInstance = NavMesh.AddNavMeshData(_collectStagePrefab.NavMeshData,
                    _collectStagePrefab.transform.position,
                    _collectStagePrefab.transform.rotation);

                _enemyFactory.Init(PauseHandler, AudioPlayerSpawner, _factoryConfig, 
                    _collectStagePrefab.Config.BossConfig, 
                    _collectStagePrefab.EnemyPatrolZones, _collectStagePrefab.BossPatrolZone);
            }
            else
            {
                Debug.LogWarning($"[{_collectStagePrefab.name}] NavMeshData not assigned!");
            }

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

            foreach (VortexTrap vortexTrap in _collectStagePrefab.VortexTraps)
                vortexTrap.Init(PauseHandler, AudioPlayerSpawner);

            _player.gameObject.SetActive(false);
            _player.transform.position = _collectStagePrefab.PlayerStartPosition;
            _player.gameObject.SetActive(true);
            Stage.Init(PauseHandler, AudioPlayerSpawner);
            _collectStagePrefab.FenceColorizer.ColorizeFence(_collectStagePrefab.ColorReference);
            StartCoroutine(DelayedCubesDropDown());
        }

        private protected override void OnLoadWindowUpdated()
        {
            base.OnLoadWindowUpdated();
            _playerInput.StartInput();
        }

        private void OnDestroy()
        {
            if (_navMeshInstance.valid)
                _navMeshInstance.Remove();
        }

        private IEnumerator DelayedCubesDropDown()
        {
            yield return new WaitForSeconds(1);

            _simpleCubeSpawner.DropCubes();
        }
    }
}