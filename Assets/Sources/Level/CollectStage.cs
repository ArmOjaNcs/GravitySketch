using Assets.Sources.Audio;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class CollectStage : Stage
    {
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private PlayerScore _playerScore;
        [SerializeField] private Player _player;
        [SerializeField] private LevelExit _exit;
        [SerializeField] private PauseableRoutine _pauseableRoutine;
        [SerializeField] private TextMeshProUGUI _finalText;
        [SerializeField] private float _timeBeforeLoad;

        private Enemy _boss;
        private float _currentEnemyDissolvedPercent;
        private float _currentCubesCountPercent;

        private protected override void Awake()
        {
            base.Awake();
            SetCurrentIndex(Index);
        }

        private protected override void OnEnable()
        {
            base.OnEnable();
            _exit.Exit += OnExitApplied;
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
            _cubesCollector.CubesCountChanged += OnCubesCountChanged;
            _pauseableRoutine.Updated += OnRoutineUpdated;
            _player.IsDead += OnPlayerDead;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _exit.Exit -= OnExitApplied;
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
            _pauseableRoutine.Updated -= OnRoutineUpdated;
            _player.IsDead -= OnPlayerDead;
        }

        public override void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Init(pauseHandler, audioPlayerSpawner);
            _player.Init(pauseHandler);
            _exit.Init(pauseHandler);
            _exit.SetAudioPlayerSpawner(audioPlayerSpawner);
            _exit.SetSize(0);
            _exit.gameObject.SetActive(false);
            _pauseableRoutine.Init(pauseHandler);
            _takeOverLimit.SetAudioPlayerSpawner(audioPlayerSpawner);
            _cubesCollector.InvokeCubesCountChanged();
            _finalText.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnRoutineUpdated()
        {
            Window.Closed += OnWindowClosed;
            Window.Hide();
        }

        private void OnWindowClosed()
        {
            Window.Closed -= OnWindowClosed;
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
            SetIntermediateResult(Index, _playerScore.Value, _cubesCollector.GetAllCollors());
            SaveProgress();
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(FinalSound).Play();
            _pauseableRoutine.UpdateView(_timeBeforeLoad);
            _finalText.text = Translator.Get(UserUtils.Great);
            _finalText.gameObject.SetActive(true);
            ToMainMenu.gameObject.SetActive(false);
            Restart.gameObject.SetActive(false);
            Window.Show();
        }

        private void OnPlayerDead()
        {
            InvokeFinished();
            _finalText.text = Translator.Get(UserUtils.GameOver);
            _finalText.color = Color.red;
            _finalText.gameObject.SetActive(true);
            Window.Show();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        private bool IsCanFinish()
        {
            return _currentEnemyDissolvedPercent >= UserUtils.MinPercentToComplete
                && _currentCubesCountPercent >= UserUtils.MinPercentToComplete;
        }

        private void UpdateExitStatus()
        {
            if (IsCanFinish() && _enemyFactory.IsBossSpawned == false)
            {
                _boss = _enemyFactory.CreateBoss();
                _boss.Finished += OnBossFinished;
            }
        }

        private void OnBossFinished()
        {
            _boss.Finished -= OnBossFinished;
            _exit.transform.position = _takeOverLimit.transform.position + Vector3.up * 70;
            _exit.gameObject.SetActive(true);
            _exit.DropDown();
        }
    }
}