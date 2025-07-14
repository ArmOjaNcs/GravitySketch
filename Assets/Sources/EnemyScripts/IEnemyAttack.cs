using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public interface IEnemyAttack
    {
        void Initialize(EnemyAttackConfig config, Transform firePoint);
    }
}