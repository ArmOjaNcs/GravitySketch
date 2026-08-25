using System;
using DG.Tweening;
using Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.AnimatorMorph
{
    public class MenuWindow : UIAnimator
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _back;
        [SerializeField] private RectTransform _startPosition;
        [SerializeField] private RectTransform _finalPosition;

        public event Action Opening;
        public event Action Opened;
        public event Action Closing;
        public event Action Closed;

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

        public void MoveToStartPosition()
        {
            if (_startPosition == null)
                return;

            SetPositionOnParentStretch(_startPosition);
        }

        public void MoveToFinalPosition()
        {
            if (_finalPosition == null)
                return;

            SetPositionOnParentStretch(_finalPosition);
        }

        private protected override void InitAnimations()
        {
            ShowAnimation = AnimationSpawner.GetMenuWindowAnimation(
                RectTransform, CanvasGroup, 0, UserUtils.DefaultStartValue, Duration);
            ShowAnimation.OnComplete(OnOpened);
            HideAnimation = AnimationSpawner.GetMenuWindowAnimation(
                RectTransform, CanvasGroup, UserUtils.DefaultStartValue, 0, Duration);
            HideAnimation.OnComplete(OnClosed);
        }

        private protected virtual void OnBackClicked()
        {
            Closing?.Invoke();
            Hide();
        }

        private protected virtual void OnOpened()
        {
            CanvasGroup.interactable = true;
            Opened?.Invoke();
        }

        private protected void OnClosed()
        {
            Closed?.Invoke();
            RectTransform.gameObject.SetActive(false);
        }

        private void SetPositionOnParentStretch(RectTransform parentRect)
        {
            RectTransform.SetParent(parentRect, false);
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.pivot = new Vector2(UserUtils.PivotCentre, UserUtils.PivotCentre);
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;
        }

        private void OnStartClicked()
        {
            Opening?.Invoke();
            Show();
        }
    }
}