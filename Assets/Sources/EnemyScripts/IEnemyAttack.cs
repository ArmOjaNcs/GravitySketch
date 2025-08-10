using Assets.Sources.Audio;
using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public interface IEnemyAttack
    {
        public void InitFromConfig(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner, PauseHandler pauseHandler);
    }
}