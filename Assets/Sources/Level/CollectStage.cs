using Assets.Sources.Audio;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class CollectStage : LevelScore
    {
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private PlayerScore _playerScore;
        [SerializeField] private LevelExit _exit;
        [SerializeField] private PauseableRoutine _pauseableRoutine;
        [SerializeField] private float _timeBeforeLoad;

        private float _currentEnemyDissolvedPercent;
        private float _currentCubesCountPercent;

        private protected override void Awake()
        {
            base.Awake();
            SetCurrentIndex(Index);
        }

        private void OnEnable()
        {
            _exit.Exit += OnExitApplied;
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
            _cubesCollector.CubesCountChanged += OnCubesCountChanged;
            _pauseableRoutine.Updated += OnRoutineUpdated;
        }

        private void OnDisable()
        {
            _exit.Exit -= OnExitApplied;
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
            _pauseableRoutine.Updated -= OnRoutineUpdated;
        }

        public void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            _exit.Init(pauseHandler);
            _exit.SetAudioPlayerSpawner(audioPlayerSpawner);
            _exit.SetSize(0);
            _exit.gameObject.SetActive(false);
            _pauseableRoutine.Init(pauseHandler);
            _takeOverLimit.SetAudioPlayerSpawner(audioPlayerSpawner);
        }

        private void OnRoutineUpdated()
        {
            SetIntermediateResult(Index, _playerScore.Value, _cubesCollector.GetAllCollors());
            SaveProgress();
            LoadNextScene();
        }

        private void OnCubesCountChanged(int cubesCount)
        {
            _currentCubesCountPercent = (float)cubesCount / _simpleCubeSpawner.TotalCubes;
            UpdateExitStatus();
        }

        private void OnEnemyDissolved()
        {
            _currentEnemyDissolvedPercent = (float)_takeOverLimit.EnemiesDissolvedCount / _enemyFactory.TotalEnemies;
            UpdateExitStatus();
        }

        private void OnExitApplied()
        {
            _pauseableRoutine.UpdateView(_timeBeforeLoad);
        }

        private bool IsCanFinish()
        {
            return _currentEnemyDissolvedPercent >= UserUtils.MinPercentToComplete
                && _currentCubesCountPercent >= UserUtils.MinPercentToComplete;
        }

        private void UpdateExitStatus()
        {
            if (IsCanFinish() && _exit.IsDowned == false)
            {
                _exit.transform.position = _takeOverLimit.transform.position + Vector3.up * 70;
                _exit.gameObject.SetActive(true);

                if (_exit.IsDowned == false)
                    _exit.DropDown();
            }
        }
    }
}