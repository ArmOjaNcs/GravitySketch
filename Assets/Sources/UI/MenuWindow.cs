using Assets.Sources.Utils;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class MenuWindow : UIAnimator
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _back;

        public event Action Opening;
        public event Action Closing;

        private void OnEnable()
        {
            if (_start != null)
                _start.onClick.AddListener(OnStartClicked);

            if (_back != null)
                _back.onClick.AddListener(OnBackClicked);
        }

        private void OnDisable()
        {
            if (_start != null)
                _start.onClick.RemoveListener(OnStartClicked);

            if (_back != null)
                _back.onClick.RemoveListener(OnBackClicked);
        }

        private protected override void InitAnimations()
        {
            ShowAnimation = AnimationSpawner.GetMenuWindowAnimation(RectTransform, CanvasGroup, 0, UserUtils.One, Duration);
            ShowAnimation.OnComplete(() => CanvasGroup.interactable = true);
            HideAnimation = AnimationSpawner.GetMenuWindowAnimation(RectTransform, CanvasGroup, UserUtils.One, 0, Duration);
            HideAnimation.OnComplete(() => RectTransform.gameObject.SetActive(false));
        }

        private void OnStartClicked()
        {
            RectTransform.gameObject.SetActive(true);
            Opening?.Invoke();
            Show();
        }

        private void OnBackClicked()
        {
            Closing?.Invoke();
            Hide();
        }
    }
}