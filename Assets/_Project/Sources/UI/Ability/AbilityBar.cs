using DG.Tweening;
using Pause;
using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;

namespace UI.Ability
{
    public abstract class AbilityBar : PauseableAnimation
    {
        [SerializeField] private protected SmoothedImage ActiveTimeImage;
        [SerializeField] private protected SmoothedImage ReloadTimeImage;
        [SerializeField] private protected RectTransform Emblem;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            ActiveTimeImage.Init(pauseHandler);
            ReloadTimeImage.Init(pauseHandler);
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetShakeAnimation(Emblem, 0.5f);
        }
    }
}