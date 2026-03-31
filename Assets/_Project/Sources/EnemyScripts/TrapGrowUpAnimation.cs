using Assets.Sources.UI;
using Assets.Sources.Utils;
using DG.Tweening;

namespace Assets.Sources.EnemyScripts
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