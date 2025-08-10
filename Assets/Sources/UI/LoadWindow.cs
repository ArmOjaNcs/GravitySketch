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
        [SerializeField] private TemplateMaterialReference _materialReference;
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
            _loadImage.color = _materialReference.GetRandomColor();
            _loadAnimation = AnimationSpawner.GetLoadAnimation(_loadImage.rectTransform, _animationDuration);
            _loadText.Init(pauseHandler);
            _loadText.Updated += OnLoadTextUpdated;
            _loadText.SignAdded += OnSignAdded;
            OnLoadTextUpdated();
            ShowElements();
            _loadAnimation.Restart();
        }

        private void OnSignAdded()
        {
            _loadImage.color = _materialReference.GetRandomColor();
        }

        private void OnLoadTextUpdated()
        {
            _count++;
            
            if(_count <= UserUtils.Three)
                _loadText.UpdateView(UserUtils.LoadTime / UserUtils.Three);
            else
                _loadAnimation.Pause();
        }
    }
}