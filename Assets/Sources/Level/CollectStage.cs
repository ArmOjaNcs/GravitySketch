using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Sources.Utils;
using Assets.Sources.EnemyScripts;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Pause;

namespace Assets.Sources.Level
{
    public class CollectStage : LevelScore
    {
        [SerializeField] private int _index;
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
            _exit.Exit += OnExitApplied;
            _exit.Init(0);
            _exit.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
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

        private void OnRoutineUpdated()
        {
            SetIntermediateResult(_index, _playerScore.Value, _cubesCollector.GetAllCollors());
            SaveProgress();
            SceneManager.LoadScene("Radar");
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
                _exit.gameObject.SetActive(true);

                if (_exit.IsDowned == false)
                    _exit.DropDown();
            }
        }
    }
}