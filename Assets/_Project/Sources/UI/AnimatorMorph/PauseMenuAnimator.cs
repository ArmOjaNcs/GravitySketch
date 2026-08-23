using System;
using Utils;
using DG.Tweening;
using UnityEngine;

namespace UI.AnimatorMorph
{
    public class PauseMenuAnimator : UIAnimator
    {
        public event Action Hidden;
        public event Action Shown;

        public override void Show()
        {
            base.Show();
            ShowAnimation.OnComplete(() =>
            {
                Shown?.Invoke();
                CanvasGroup.interactable = true;
            });
        }

        public override void Hide()
        {
            base.Hide();
            HideAnimation.OnComplete(() => Hidden?.Invoke());
        }

        private protected override void InitAnimations()
        {
            ShowAnimation = AnimationSpawner.GetOptionsShowAnimation(RectTransform, CanvasGroup, Duration);
            HideAnimation = AnimationSpawner.GetOptionsHideAnimation(RectTransform, CanvasGroup, Duration);
        }
    }
}