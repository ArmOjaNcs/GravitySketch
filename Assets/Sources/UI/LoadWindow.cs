using Assets.Sources.Pause;
using Assets.Sources.Table;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class LoadWindow : SmoothedFade
    {
        [SerializeField] private TemplateColorReference _colorReference;
        [SerializeField] private Image _cubeImage;
        [SerializeField] private Sprite[] _loadSprites;
        [SerializeField] private Image _loadImage;
        [SerializeField] private AppearingText _loadText;
        [SerializeField] private float _animationDuration;

        private Tween _loadAnimation;
        private int _count;

        private protected override void OnDisable()
        {
            _loadText.Updated -= OnLoadTextUpdated;
            _loadText.SignAdded -= OnSignAdded;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _cubeImage.color = _colorReference.GetRandomColor();
            _loadAnimation = AnimationSpawner.GetLoadAnimation(_cubeImage.rectTransform, _animationDuration);
            _loadText.Init(pauseHandler);
            _loadText.Updated += OnLoadTextUpdated;
            _loadText.SignAdded += OnSignAdded;
            OnLoadTextUpdated();
            ShowElements();
            _loadAnimation.Restart();
            int random = Random.Range(0, _loadSprites.Length);
            _loadImage.sprite = _loadSprites[random];
        }

        private void OnSignAdded()
        {
            _cubeImage.color = _colorReference.GetRandomColor();
        }

        private void OnLoadTextUpdated()
        {
            _count++;

            if (_count <= UserUtils.One)
                _loadText.UpdateView(UserUtils.LoadTime);
            else
                _loadAnimation.Pause();
        }
    }
}