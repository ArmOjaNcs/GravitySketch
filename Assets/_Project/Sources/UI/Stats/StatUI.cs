using DG.Tweening;
using Utils;
using UnityEngine;

namespace UI.Stats
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