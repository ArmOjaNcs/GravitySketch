using Assets.Sources.Audio;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Save;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Table;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField] private Grower _grower;
        [SerializeField] private LevelExit _exit;
        [SerializeField] private HoleMaskHandler _maskHandler;
        [SerializeField] private PauseableRoutine _finishRoutine;
        [SerializeField] private TextMeshProUGUI _finalText;
        [SerializeField] private FixedJoystick _moveJoystick;
        [SerializeField] private FixedJoystick _rotateJoystick;
        [SerializeField] private Button _shieldAbilityButton;
        [SerializeField] private Button _boostAbilityButton;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _timeBeforeLoad;
        [SerializeField] private Image _textBackground;
        
        private Enemy _boss;
        private float _currentEnemyDissolvedPercent;
        private float _currentCubesCountPercent;
        private TutorialHandler _tutorialHandler;
        private Color _textBackgroundDefaultColor;

        private protected override void Awake()
        {
            base.Awake();

            if (IsTutorial)
                _player.SetTutorial();

            _reviveButton.gameObject.SetActive(false);
            _textBackgroundDefaultColor = _textBackground.color;
        }

        private protected override void OnEnable()
        {
            base.OnEnable();
            _exit.Exit += OnExitApplied;
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
            _cubesCollector.CubesCountChanged += OnCubesCountChanged;
            _finishRoutine.Updated += OnFinishRoutineUpdated;
            _player.IsDead += OnPlayerDead;
            _reviveButton.onClick.AddListener(OnReviveButtonClicked);
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _exit.Exit -= OnExitApplied;
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
            _finishRoutine.Updated -= OnFinishRoutineUpdated;
            _player.IsDead -= OnPlayerDead;
            _reviveButton.onClick.RemoveListener(OnReviveButtonClicked);

            if (_tutorialHandler != null)
            {
                _tutorialHandler.Triggered -= OnTutorialHandlerTriggered;
                _tutorialHandler.TutorialViewClosed -= OnTutorialViewClosed;
            }
        }

        public override void SetTutorialObject(GameObject tutorialObject)
        {
            base.SetTutorialObject(tutorialObject);
            _tutorialHandler = TutorialObject.GetComponent<TutorialHandler>();

            if(_tutorialHandler != null)
            {
                _tutorialHandler.Triggered += OnTutorialHandlerTriggered;
                _tutorialHandler.TutorialViewClosed += OnTutorialViewClosed;
            }
        }

        public override void Begin()
        {
            _grower.Updated += OnStartGrowerUpdated;
            _grower.GrowTo(Vector3.one, true);
        }

        private void OnStartGrowerUpdated()
        {
            if (IsTutorial)
                _tutorialHandler.StartTutorial();

            _grower.Updated -= OnStartGrowerUpdated;
            _playerInput.StartInput();
            base.Begin();
        }

        private void OnTutorialViewClosed()
        {
            PauseHandler.Resume();
            Pause.gameObject.SetActive(true);
        }

        private void OnTutorialHandlerTriggered()
        {
            Pause.gameObject.SetActive(false);
            PauseHandler.Pause();
            _tutorialHandler.Show();
        }

        public override void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Init(pauseHandler, audioPlayerSpawner);
            _playerInput.InitBindings(Bindings, _moveJoystick, _rotateJoystick,
                _shieldAbilityButton, _boostAbilityButton);
            _shieldAbilityButton.gameObject.SetActive(false);
            _boostAbilityButton.gameObject.SetActive(false);
            _player.Init(pauseHandler);
            _exit.Init(pauseHandler);
            _exit.SetAudioPlayerSpawner(audioPlayerSpawner);
            _exit.SetSize(0);
            _exit.gameObject.SetActive(false);
            _finishRoutine.Init(pauseHandler);
            _cubesCollector.InvokeCubesCountChanged();
            _finalText.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnReviveButtonClicked()
        {
            TextWindow.Closed += OnReviveWindowClosed;
            TextWindow.Hide();
            ButtonsWindow.Hide();
        }

        private void OnReviveWindowClosed()
        {
            TextWindow.Closed -= OnReviveWindowClosed;
            _reviveButton.gameObject.SetActive(false);
            _finalText.gameObject.SetActive(false);
            _grower.Updated += OnReviveGrowerUpdated;
            _grower.GrowTo(Vector3.one * (_player.CurrentSize / 2) + Vector3.one, true);
            
        }

        private void OnReviveGrowerUpdated()
        {
            _grower.Updated -= OnReviveGrowerUpdated;
            RevivePlayer();
        }

        private void RevivePlayer()
        {
            _player.Revive();
            Pause.gameObject.SetActive(true);
            _playerInput.StartInput();
            PauseInput.StartInput();
            PauseInput.Paused += OnPaused;
        }

        private void OnFinishRoutineUpdated()
        {
            Progress.SetIntermediateResult(_playerScore.Value, _cubesCollector.GetAllCollors());
            Progress.SetStageName(UserUtils.GetPaintStageName(StageName));
            Progress.SetSceneType(SceneType.Paint);
            SaveSystem.SavePlayerProgress(Progress);
            TextWindow.Closed += OnFinalWindowClosed;
            TextWindow.Hide();
            ButtonsWindow.Hide();
        }

        private protected override void OnVirtualJoystickValueChanged(bool value)
        {
            _shieldAbilityButton.gameObject.SetActive(value);
            _shieldAbilityButton.interactable = value;
            _boostAbilityButton.gameObject.SetActive(value);
            _boostAbilityButton.interactable = value;
            _moveJoystick.gameObject.SetActive(value);
            _rotateJoystick.gameObject.SetActive(value);
            base.OnVirtualJoystickValueChanged(value);
        }

        private void OnFinalWindowClosed()
        {
            TextWindow.Closed -= OnFinalWindowClosed;
            SceneManager.LoadScene(UserUtils.Load);
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
            Finish();
            _player.SetFinished();
            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(FinalSound)?.Play();
            _finishRoutine.UpdateView(_timeBeforeLoad);
            _finalText.text = Translator.Get(UserUtils.Great);
            _finalText.gameObject.SetActive(true);
            _textBackground.color = Color.clear;
            ToMainMenu.gameObject.SetActive(false);
            Restart.gameObject.SetActive(false);
            TextWindow.Show();
        }

        private void OnPlayerDead()
        {
            Finish();
            _playerInput.StopInput();
            _grower.GrowTo(Vector3.zero);
            _finalText.text = Translator.Get(UserUtils.GameOver);
            _finalText.color = Color.red;
            _finalText.gameObject.SetActive(true);
            Pause.gameObject.SetActive(false);
            TextWindow.Show();
            ButtonsWindow.Show();
            _reviveButton.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        private bool IsCanFinish()
        {
            return _currentCubesCountPercent >= UserUtils.MinPercentToComplete;
        }

        private void UpdateExitStatus()
        {
            if (IsCanFinish() && _enemyFactory.IsBossSpawned == false)
            {
                _boss = _enemyFactory.CreateBoss();
                _boss.Downed += OnBossDowned;
            }
        }

        private void OnBossDowned()
        {
            _boss.Downed -= OnBossDowned;
            _exit.transform.position = _takeOverLimit.transform.position + Vector3.up * 70;
            _exit.gameObject.SetActive(true);
            _exit.DropDown();
        }
    }
}