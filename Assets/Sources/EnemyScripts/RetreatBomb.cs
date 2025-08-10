using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class RetreatBomb : Bomb
    {
        public override void InitFromConfig(MissileConfig config, EnemyAttackZone attackZone)
        {
            base.InitFromConfig(config, attackZone);
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