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
        [SerializeField] private Button _tutorial;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Toggle[] _toggles;
        [SerializeField] private AudioSource _buttonSound;
        [SerializeField] private AudioSource _toggleSound;

        private bool _isStarted;

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
            _tutorial.onClick.AddListener(OnTutorialClicked);
        }

        private void OnDisable()
        {
            _levelSelector.PlayClicked -= OnPlayClicked;
            _start.onClick.RemoveListener(OnStartClicked);
            _tutorial.onClick.RemoveListener(OnTutorialClicked);

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

            if (IsTutorial)
                Progress.SetTutorial(false);

            SaveSystem.SavePlayerProgress(Progress);
            _default.Show();
        }

        private void OnWindowOpening() => _default.Hide();
        private void OnWindowClosing() => _default.Show();

        private void OnPlayClicked(string stageName)
        {
            Progress.SetStageName(stageName);
            SaveSystem.SavePlayerProgress(Progress);
            SceneManager.LoadScene(UserUtils.Collect);
        }

        private void OnStartClicked() => SceneManager.LoadScene(UserUtils.Collect);

        private void OnTutorialClicked()
        {
            Progress.SetTutorial(true);
            SaveSystem.SavePlayerProgress(Progress);
            OnStartClicked();
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
            _startButtonText.text = Translator.Get(text);
            _levelSelector.Init(this);
            _levelSelector.PlayClicked += OnPlayClicked;
            _isStarted = true;
        }
    }
}