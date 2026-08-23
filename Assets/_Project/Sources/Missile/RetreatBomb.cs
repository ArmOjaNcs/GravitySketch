using EnemyScripts.EnemyZones;
using Missile.Configs;
using Utils;
using UnityEngine;

namespace Missile
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
            Radius /= UserUtils.Two;
        }
    }
}