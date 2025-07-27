using Assets.Sources.Utils;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class UIAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _duration;

        private CanvasGroup _canvasGroup;
        private Sequence _showAnimation;
        private Sequence _hideAnimation;

        public event Action Hidden;

        public bool IsShown { get; private set; }

        private void Awake()
        {
            _canvasGroup = _rectTransform.GetComponent<CanvasGroup>();
            InitAnimations();
        }

        private void OnDestroy()
        {
            _showAnimation?.Kill();
            _hideAnimation?.Kill();
        }

        public void Show()
        {
            IsShown = true;
            _hideAnimation?.Pause();
            gameObject.SetActive(true);
            _showAnimation?.Restart();
        }

        public void Hide()
        {
            IsShown = false;
            _showAnimation?.Pause();
            _canvasGroup.interactable = false;
            _hideAnimation?.Restart();
            _hideAnimation.OnComplete(() => Hidden?.Invoke());
        }

        private void InitAnimations()
        {
            _showAnimation = AnimationSpawner.GetShowAnimation(_rectTransform, _duration);
            _hideAnimation = AnimationSpawner.GetHideAnimation(_rectTransform, _duration);
        }
    }
}