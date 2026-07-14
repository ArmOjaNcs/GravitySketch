using Assets.Sources.Audio;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Save;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Table;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Assets.Sources.Level
{
    public class CollectStage : Stage
    {
        private const float RewardDelay = 120f;

        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private PlayerScore _playerScore;
        [SerializeField] private Player _player;
        [SerializeField] private Grower _grower;
        [SerializeField] private LevelExit _exit;
        [SerializeField] private HoleMaskHandler _maskHandler;
        [SerializeField] private WaitRoutine _waitRoutine;
        [SerializeField] private TextMeshProUGUI _finalText;
        [SerializeField] private DynamicJoystick _moveJoystick;
        [SerializeField] private DynamicJoystick _rotateJoystick;
        [SerializeField] private Button _shieldAbilityButton;
        [SerializeField] private Button _boostAbilityButton;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private MenuWindow _reviveButtonAnimator;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Image _textBackground;
        [SerializeField] private MusicPlayer _musicPlayer;

        private Enemy _boss;
        private float _currentEnemyDissolvedPercent;
        private float _currentCubesCountPercent;
        private TutorialHandler _tutorialHandler;
        private Color _textBackgroundDefaultColor;
        private bool _isRewardAvailable;

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
            _player.IsDead += OnPlayerDead;
            _reviveButton.onClick.AddListener(OnReviveButtonClicked);
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _exit.Exit -= OnExitApplied;
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
            _player.IsDead -= OnPlayerDead;
            _reviveButton.onClick.RemoveListener(OnReviveButtonClicked);

            if (_tutorialHandler != null)
            {
                _tutorialHandler.IsAccepted -= OnTutorialStartWindowClosed;
                _tutorialHandler.Triggered -= OnTutorialHandlerTriggered;
                _tutorialHandler.TutorialViewClosed -= OnTutorialViewClosed;
            }
        }

        public override void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Init(pauseHandler, audioPlayerSpawner);
            _playerInput.InitBindings(
                Bindings, _moveJoystick, _rotateJoystick, _shieldAbilityButton, _boostAbilityButton);
            _shieldAbilityButton.gameObject.SetActive(false);
            _boostAbilityButton.gameObject.SetActive(false);
            _player.Init(pauseHandler);
            _exit.Init(pauseHandler);
            _exit.SetAudioPlayerSpawner(audioPlayerSpawner);
            _exit.SetSize(0);
            _exit.gameObject.SetActive(false);
            _waitRoutine.Init(pauseHandler);
            _isRewardAvailable = true;
            _cubesCollector.InvokeCubesCountChanged();

            if (_playerInput.IsJoystickMode)
                return;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override void SetTutorialObject(GameObject tutorialObject)
        {
            base.SetTutorialObject(tutorialObject);
            _tutorialHandler = TutorialObject.GetComponent<TutorialHandler>();

            if (_tutorialHandler != null)
            {
                _tutorialHandler.IsAccepted += OnTutorialStartWindowClosed;
                _tutorialHandler.Triggered += OnTutorialHandlerTriggered;
                _tutorialHandler.TutorialViewClosed += OnTutorialViewClosed;
            }
        }

        public override void Begin()
        {
            _grower.Updated += OnStartGrowerUpdated;
            _grower.GrowTo(Vector3.one, true);
        }

        private protected override void HideButtons()
        {
            if (TextWindow.IsShown && _reviveButton.gameObject.activeSelf)
                _reviveButtonAnimator.Hide();

            base.HideButtons();
        }

        private protected override void OnPauseMenuClosed()
        {
            base.OnPauseMenuClosed();

            if (_playerInput.IsJoystickMode)
                return;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

        private void OnStartGrowerUpdated()
        {
            if (IsTutorial && _tutorialHandler != null)
            {
                PauseHandler.Pause();
                Pause.interactable = false;
                _tutorialHandler.Begin();
            }

            _grower.Updated -= OnStartGrowerUpdated;
            _playerInput.StartInput();
            base.Begin();
        }

        private void OnTutorialViewClosed()
        {
            PauseHandler.Resume();
            Pause.interactable = true;
        }

        private void OnTutorialHandlerTriggered()
        {
            Pause.interactable = false;
            PauseHandler.Pause();
            _tutorialHandler.Show();
        }

        private void OnReviveButtonClicked()
        {
            _waitRoutine.Updated += OnRewardRoutineUpdated;
            _isRewardAvailable = false;
            _waitRoutine.Wait(RewardDelay);
            PauseHandler.Pause();
            _reviveButtonAnimator.Hide();
            _musicPlayer.Stop();
            YG2.RewardedAdvShow(string.Empty, () => StartCoroutine(RefreshEventSystem(CloseReviveButtons)));
        }

        private void CloseReviveButtons()
        {
            Buttons[0].Closed += OnReviveButtonsClosed;
            HideButtons();
        }

        private void OnReviveButtonsClosed()
        {
            Buttons[0].Closed -= OnReviveButtonsClosed;

            foreach (MenuWindow buttonAnimator in Buttons)
                buttonAnimator.MoveToStartPosition();

            TextWindow.Closed += OnReviveWindowClosed;
            TextWindow.Hide();
            _reviveButton.gameObject.SetActive(false);
            _musicPlayer.PlayRandomMusic();
        }

        private void OnReviveWindowClosed()
        {
            TextWindow.Closed -= OnReviveWindowClosed;
            _grower.Updated += OnReviveGrowerUpdated;
            PauseHandler.Resume();
            _grower.GrowTo((Vector3.one * (_player.CurrentSize / 2)) + Vector3.one, true);
        }

        private void OnReviveGrowerUpdated()
        {
            _grower.Updated -= OnReviveGrowerUpdated;
            RevivePlayer();
        }

        private void RevivePlayer()
        {
            _player.Revive();
            Pause.interactable = true;
            _playerInput.StartInput();
            PauseInput.StartInput();
            PauseInput.Paused += OnPaused;
        }

        private void OnAcceptButtonClicked()
        {
            _acceptButton.onClick.RemoveListener(OnAcceptButtonClicked);
            TryShowAdv(() => SceneManager.LoadScene(UserUtils.Load));
        }

        private void OnRewardRoutineUpdated()
        {
            _waitRoutine.Updated -= OnRewardRoutineUpdated;
            _isRewardAvailable = true;
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
            _finalText.SetText(Translator.Get(UserUtils.Great));
            _textBackground.color = Color.green;
            Pause.interactable = false;
            PauseHandler.Pause();
            Progress.SetIntermediateResult(_playerScore.Value, _cubesCollector.GetAllCollors());
            Progress.SetStageName(UserUtils.GetPaintStageName(StageName));
            Progress.SetSceneType(SceneType.Paint);
            SaveSystem.SavePlayerProgress(Progress);
            TextWindow.Show();
            _acceptButton.gameObject.SetActive(true);
            _acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        }

        private void OnPlayerDead()
        {
            Finish();
            _playerInput.StopInput();
            _grower.GrowTo(Vector3.zero);
            _finalText.SetText(Translator.Get(UserUtils.GameOver));
            _finalText.color = Color.red;
            Pause.interactable = false;

            foreach (MenuWindow button in Buttons)
                button.MoveToFinalPosition();

            TextWindow.Opened += OnTextWindowOpened;
            TextWindow.Show();
        }

        private void OnTextWindowOpened()
        {
            TextWindow.Opened -= OnTextWindowOpened;
            ShowButtons();

            if (_isRewardAvailable)
            {
                _reviveButton.gameObject.SetActive(true);
                _reviveButtonAnimator.Show();
            }

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
            _exit.transform.position = _takeOverLimit.transform.position + (Vector3.up * 70);
            _exit.gameObject.SetActive(true);
            _exit.DropDown();
        }

        private void OnTutorialStartWindowClosed(bool isAccepted)
        {
            Pause.interactable = true;
            PauseHandler.Resume();

            if (isAccepted)
                _tutorialHandler.StartTutorial();
        }
    }
}