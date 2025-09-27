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
        [SerializeField] private AudioSource _buttonSound;

        private void OnEnable()
        {
            foreach (MenuWindow menuWindow in _windows)
            {
                menuWindow.Opening += OnWindowOpening;
                menuWindow.Closing += OnWindowClosing;
            }

            foreach (Button button in _buttons)
                button.onClick.AddListener(OnButtonClick);

            _start.onClick.AddListener(OnStartClicked);
        }

        private void OnDisable()
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
        }

        private void Start()
        {
            if (YandexGame.EnvironmentData == null)
                YandexGame.GetDataEvent += OnYGReady;
            else
                OnYGReady();

            Progress.SetStageName(UserUtils.GetCollectStageName(StageName));
            _default.Show();
        }

        private void OnWindowOpening() => _default.Hide();
        private void OnWindowClosing() => _default.Show();

        private void OnPlayClicked(string stageName)
        {
            Progress.SetStageName(stageName);
            SceneManager.LoadScene(UserUtils.Collect);
        }

        private void OnStartClicked() => SceneManager.LoadScene(UserUtils.Collect);

        private void OnButtonClick() => _buttonSound.Play();

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
        }
    }
}