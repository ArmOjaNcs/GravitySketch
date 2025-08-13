using Assets.Sources.Utils;
using DG.Tweening;
using System;

namespace Assets.Sources.UI
{
    public class PauseMenuAnimator : UIAnimator
    {
        public event Action Hidden;

        private protected override void InitAnimations()
        {
            ShowAnimation = AnimationSpawner.GetOptionsShowAnimation(RectTransform, Duration);
            HideAnimation = AnimationSpawner.GetOptionsHideAnimation(RectTransform, Duration);
        }

        public override void Show()
        {
            base.Show();
            ShowAnimation.OnComplete(() => CanvasGroup.interactable = true);
        }

        public override void Hide()
        {
            base.Hide();
            HideAnimation.OnComplete(() => Hidden?.Invoke());
        }

        public void BaseHide()=> base.Hide();
    }
}