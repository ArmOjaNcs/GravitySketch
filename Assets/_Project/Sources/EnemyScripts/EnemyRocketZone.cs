using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyRocketZone : EnemyAttackZone
    {
        private ObjectPool<Rocket> _pool;
        private RocketConfig _rocketConfig;

        public override void InitFromConfig(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner, PauseHandler pauseHandler)
        {
            base.InitFromConfig(config, firePoint, audioPlayerSpawner, pauseHandler);

            RocketerConfig rocketerConfig = config.SafeCast<RocketerConfig>();

            if(rocketerConfig != null)
            {
                _pool = new ObjectPool<Rocket>(rocketerConfig.RocketPrefab, rocketerConfig.Capacity, transform);
                _rocketConfig = rocketerConfig.RocketConfig;
                IsInitialized = true;
                return;
            }
        }

        private protected override void Attack()
        {
            base.Attack();

            Rocket rocket = _pool.GetElement();
            
            if (rocket.IsInitialized == false)
            {
                rocket.InitFromConfig(_rocketConfig, this);
                rocket.Init(PauseHandler);
            }

            rocket.transform.position = FirePoint.position;
            rocket.gameObject.SetActive(true);
            AudioPlayer?.Play();
            rocket.Launch();
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/Rocket/RocketLauncher");
        }
    }
}