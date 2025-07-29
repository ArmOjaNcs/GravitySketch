using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public interface IEnemyAttack
    {
        public void Initialize(EnemyAttackConfig config, Transform firePoint);
    }
}