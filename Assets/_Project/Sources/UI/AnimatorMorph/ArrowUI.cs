using Utils;
using UnityEngine;

namespace UI.AnimatorMorph
{
    public class ArrowUI : UIAnimator
    {
        [SerializeField] private Vector2 _offset;

        private protected override void InitAnimations()
        {
            ShowAnimation = AnimationSpawner.GetMoveScaleAnimation(RectTransform, _offset);
            HideAnimation = AnimationSpawner.GetFadeAnimation(CanvasGroup, 1, 0, Duration);
        }
    }
}