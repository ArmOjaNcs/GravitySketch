using Assets.Sources.UI;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public class MainMenu : LevelScore
    {
        [SerializeField] private LevelSelector _levelSelector;
        [SerializeField] private TextMeshProUGUI _startButtonText;
        [SerializeField] private MenuWindow _default;
        [SerializeField] private MenuWindow[] _windows;
        [SerializeField] private Button _start;

        private protected override void Awake()
        {
            base.Awake();

            if (LevelsCount == 0)
                _startButtonText.text = UserUtils.Start;
            else
                _startButtonText.text = UserUtils.Resume;

            _levelSelector.Init(this);
            _levelSelector.PlayClicked += OnPlayClicked;
        }

        private void OnEnable()
        {
            foreach(MenuWindow menuWindow in _windows)
            {
                menuWindow.Opening += OnWindowOpening;
                menuWindow.Closing += OnWindowClosing;
            }

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
        }

        private void Start()
        {
            _default.Show();
        }

        private void OnWindowOpening() => _default.Hide();
        private void OnWindowClosing() => _default.Show();

        private void OnPlayClicked(string sceneName) => LoadScene(sceneName);

        private void OnStartClicked()
        {
            UserUtils.TryGetSceneName(CurrentLevelIndex, out string sceneName);
            LoadScene(sceneName);
        }
    }
}