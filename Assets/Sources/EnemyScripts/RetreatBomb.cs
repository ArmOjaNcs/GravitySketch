using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class RetreatBomb : Bomb
    {
        public override void Initialize(MissileConfig config, EnemyAttackZone attackZone)
        {
            base.Initialize(config, attackZone);
            CorrectSize();
        }

        private void CorrectSize()
        {
            Transform.localScale /= UserUtils.Two;
            Damage /= UserUtils.Two;
            Force /= UserUtils.Two;
            Radius /= UserUtils.Two;
        }
    }
}