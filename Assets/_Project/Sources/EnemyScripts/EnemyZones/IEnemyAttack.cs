using Audio;
using EnemyScripts.Configs;
using Pause;
using UnityEngine;

namespace EnemyScripts.EnemyZones
{
    public interface IEnemyAttack
    {
        public void InitFromConfig(
            EnemyAttackConfig config,
            Transform firePoint,
            AudioPlayerSpawner audioPlayerSpawner,
            PauseHandler pauseHandler);
    }
}