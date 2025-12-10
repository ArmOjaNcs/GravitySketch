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
        public event Action Closed;

        private void OnEnable()
        {
            if (_start != null)
                _start.onClick.AddListener(OnStartClicked);

            if (_back != null)
                _back.onClick.AddListener(OnBackClicked);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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
            HideAnimation.OnComplete(() => OnAnimationComplete());
        }

        private void OnStartClicked()
        {
            RectTransform.gameObject.SetActive(true);
            Opening?.Invoke();
            Show();
        }

        private protected virtual void OnBackClicked()
        {
            Closing?.Invoke();
            Hide();
        }

        private void OnAnimationComplete()
        {
            Closed?.Invoke();
            RectTransform.gameObject.SetActive(false);
        }
    }
}