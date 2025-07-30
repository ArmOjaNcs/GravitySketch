using Assets.Sources.Audio;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyBomberZone : EnemyAttackZone
    {
        private ObjectPool<ThrowableBomb> _pool;
        private BombConfig _config;

        public override void Initialize(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Initialize(config, firePoint, audioPlayerSpawner);

            BomberConfig bomberConfig = config.SafeCast<BomberConfig>();

            if (bomberConfig != null)
            {
                _pool = new ObjectPool<ThrowableBomb>(bomberConfig.BombPrefab, bomberConfig.Capacity, transform);
                _config = bomberConfig.BombConfig;
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
        }

        private protected override void Attack()
        {
            base.Attack();

            ThrowableBomb bomb = _pool.GetElement();
   
            if (bomb.IsInitialized == false)
                bomb.Initialize(_config, this);

            bomb.transform.position = FirePoint.position;
            bomb.gameObject.SetActive(true);
            AudioPlayer.Play();
            bomb.AddForces(FirePoint.position);
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/Bomb/GrenadeLauncher");
        }
    }
}