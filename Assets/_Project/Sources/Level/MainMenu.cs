using Assets.Sources.Save;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private AudioSource _backgroundMusic;
        [SerializeField] private LeaderboardYG _leaderboard;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSettingsMenu _inputSettings;

        private bool _isStarted;

        private void OnEnable()
        {
            Subscribe();

            if (YandexGame.EnvironmentData != null)
                OnYGReady();
            else
                YandexGame.GetDataEvent += OnYGReady;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Start()
        {
            Progress.SetStageName(UserUtils.GetCollectStageName(StageName));
            Progress.SetSceneType(SceneType.Collect);
            _isStarted = true;
            StartCoroutine(RefreshEventSystem());
            _leaderboard.UpdateLB();
            _backgroundMusic.Play();
        }

        private void OnWindowOpening() => _default.Hide();
        private void OnWindowClosing() => _default.Show();

        private void OnPlayClicked(string stageName)
        {
            Progress.SetStageName(stageName);
            OnStartClicked();
        }

        private void OnStartClicked()
        {
            if (Progress.StageName.Equals(UserUtils.TutorialCollectName))
                Progress.SetTutorial(true);
            else
                Progress.SetTutorial(false);

            SaveSystem.SavePlayerProgress(Progress);
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
            YandexGame.GetDataEvent -= OnYGReady;
            string text = LevelsCount == 0 ? UserUtils.Start : UserUtils.Continue;
            Translator.UpdateLang();
            _startButtonText.text = Translator.Get(text);
            StartCoroutine(DelayedStart());
        }

        private void Subscribe()
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
        }

        private void UnSubscribe()
        {
            _levelSelector.PlayClicked -= OnPlayClicked;
            _start.onClick.RemoveListener(OnStartClicked);

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

        private IEnumerator RefreshEventSystem()
        {
            yield return new WaitForEndOfFrame();
            _eventSystem.enabled = false;
            _eventSystem.enabled = true;
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(1);
            GameObject cubesPrefab = Resources.Load<GameObject>(UserUtils.GetToyCubeHolderName(UserUtils.Main));
            cubesPrefab = Instantiate(cubesPrefab);
            yield return null;
            _levelSelector.Init(this);
            _levelSelector.PlayClicked += OnPlayClicked;
            yield return null;
            _leaderboard.SetLeaderboard(TotalScore);
            SaveSystem.SavePlayerProgress(Progress);
            yield return null;
            _inputSettings.Rebuild();
            yield return null;
            _default.Show();
        }
    }
}