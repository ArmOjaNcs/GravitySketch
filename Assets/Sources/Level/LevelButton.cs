using Assets.Sources.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class LevelButton : UIAnimator, IDisposable
    {
        [SerializeField] private int _index;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _colored;
        [SerializeField] private Sprite _frame;

        public event Action<LevelButton> Chosen;

        public string Name { get; private set; }
        public int Index => _index;

        public void Init()
        {
            UserUtils.TryGetSceneName(_index, out string sceneName);
            Name = sceneName;
            _button.onClick.AddListener(OnClick);
            _score.text = "";
            _score.gameObject.SetActive(true);
            _image.sprite = _frame;
        }

        public void Dispose() => _button.onClick.RemoveListener(OnClick);

        public void SetScore(int score) => _score.text = score.ToString();

        private void OnClick() => Chosen?.Invoke(this);

        private protected override void InitAnimations()
        {
            ShowAnimation = AnimationSpawner.GetFadeAnimation(CanvasGroup, 0, UserUtils.One, 0.5f);
            HideAnimation = AnimationSpawner.GetFadeAnimation(CanvasGroup, UserUtils.One, 0, 0.5f);
        }

        public override void Show()
        {
            base.Show();
            _image.sprite = _colored;
        }

        public override void Hide()
        {
            base.Hide();
            _image.sprite = _frame;
        }
    }
}