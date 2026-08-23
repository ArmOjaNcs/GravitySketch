using DG.Tweening;
using UI;
using Utils;

namespace Trap
{
    public class TrapGrowUpAnimation : PauseableAnimation
    {
        private float _defaultSize;

        private void Awake()
        {
            _defaultSize = transform.localScale.x;
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetTrapGrowUpAnimation(transform, _defaultSize);
        }
    }
}