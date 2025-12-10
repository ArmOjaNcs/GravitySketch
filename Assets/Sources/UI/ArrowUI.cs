using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
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