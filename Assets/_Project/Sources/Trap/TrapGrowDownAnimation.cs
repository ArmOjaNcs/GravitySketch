using DG.Tweening;
using UI;
using Utils;

namespace Trap
{
    public class TrapGrowDownAnimation : PauseableAnimation
    {
        private float _defaultSize;

        private void Awake()
        {
            _defaultSize = transform.localScale.x;
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetTrapGrowDownAnimation(transform, _defaultSize);
        }
    }
}