using Assets.Sources.Save;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Assets.Sources.Level
{
    public class MainMenu : LevelScore
    {
        [SerializeField] private LevelSelector _levelSelector;
        [SerializeField] private TextMeshProUGUI _startButtonText;
        [SerializeField] private MenuWindow _default;
        [SerializeField] private MenuWindow[] _windows;
        [SerializeField] private Button _start;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Toggle[] _toggles;
        [SerializeField] private AudioSource _buttonSound;
        [SerializeField] private AudioSource _toggleSound;
        [SerializeField] private LeaderboardYG _leaderboard;

        private bool _isStarted;
        private MenuWindow _leaderboardView;

        private void OnEnable()
        {
            foreach (MenuWindow menuWindow in _windows)
            {
                menuWindow.Opening += OnWindowOpening;
                menuWindow.Closing += OnWindowClosing;
            }

            foreach (Button button in _buttons)
                button.onClick.AddListener(OnButtonClick);

            foreach (Toggle toggle in _toggles)
                toggle.onValueChanged.AddListener(OnToggleChanged);

            _start.onClick.AddListener(OnStartClicked);
            _leaderboardView = _leaderboard.GetComponent<MenuWindow>();
            _leaderboardView.Opening += OnLeaderboardOpening;
        }

        private void OnDisable()
        {
            _levelSelector.PlayClicked -= OnPlayClicked;
            _start.onClick.RemoveListener(OnStartClicked);
            _leaderboardView.Opening -= OnLeaderboardOpening;

            foreach (MenuWindow menuWindow in _windows)
            {
                menuWindow.Opening -= OnWindowOpening;
                menuWindow.Closing -= OnWindowClosing;
            }

            foreach (Button button in _buttons)
                button.onClick.RemoveListener(OnButtonClick);

            foreach (Toggle toggle in _toggles)
                toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        private void Start()
        {
            if (YandexGame.EnvironmentData == null)
                YandexGame.GetDataEvent += OnYGReady;
            else
                OnYGReady();

            Progress.SetStageName(UserUtils.GetCollectStageName(StageName));
            Progress.SetSceneType(SceneType.Collect);

            if (StageName.Equals(UserUtils.TutorialCollectName) && IsTutorial == false)
                Progress.SetTutorial(true);

            YandexGame.NewLeaderboardScores("Leaderboard", TotalScore);
            SaveSystem.SavePlayerProgress(Progress);
            _default.Show();
        }

        private void OnWindowOpening() => _default.Hide();
        private void OnWindowClosing() => _default.Show();
        private void OnLeaderboardOpening() => _leaderboard.UpdateLB();

        private void OnPlayClicked(string stageName)
        {
            if (stageName.Equals(UserUtils.TutorialCollectName))
                Progress.SetTutorial(true);
            else
                Progress.SetTutorial(false);

            Progress.SetStageName(stageName);
            SaveSystem.SavePlayerProgress(Progress);
            SceneManager.LoadScene(UserUtils.Load);
        }

        private void OnStartClicked()
        {
            if(StageName.Equals(UserUtils.TutorialCollectName) == false)
            {
                Progress.SetTutorial(false);
                SaveSystem.SavePlayerProgress(Progress);
            }

            SceneManager.LoadScene(UserUtils.Load);
        }

        private void OnButtonClick() => _buttonSound.Play();
        private void OnToggleChanged(bool value)
        {
            if (_isStarted == false)
                return;
            
            _toggleSound.Play();
        } 

        private void OnYGReady()
        {
            string text = string.Empty;

            if (LevelsCount == 0)
                text = UserUtils.Start;
            else
                text = UserUtils.Continue;

            Translator.UpdateLang();
            GameObject cubesPrefab = Resources.Load<GameObject>(UserUtils.GetToyCubeHolderName(UserUtils.Main));
            cubesPrefab = Instantiate(cubesPrefab);
            _startButtonText.text = Translator.Get(text);
            _levelSelector.Init(this);
            _levelSelector.PlayClicked += OnPlayClicked;
            _isStarted = true;
        }
    }
}