using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class StatUI : PauseableAnimation
    {
        [SerializeField] private RectTransform _rectTransform;

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetStatAnimation(_rectTransform, 2f);
        }
    }
}