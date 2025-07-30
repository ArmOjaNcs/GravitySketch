using Assets.Sources.Audio;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemySniperZone : EnemyAttackZone
    {
        private AimCross _cross;
        private bool _isCanShoot = true;

        private void OnDisable()
        {
            if(_cross != null)
                _cross.Shoot -= OnShoot;
        }

        private protected override void Update()
        {
            if (_isCanShoot == false)
                return;

            base.Update();
        }

        public override void Initialize(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Initialize(config, firePoint, audioPlayerSpawner);

            SniperConfig sniperConfig = config.SafeCast<SniperConfig>();

            if(sniperConfig != null)
            {
                _cross = Instantiate(sniperConfig.AimCrossPrefab).GetComponent<AimCross>();
                _cross.Initialize(sniperConfig.AimCrossConfig, this);
                _cross.Shoot += OnShoot;
                Return(_cross.gameObject);
                IsInitialized = true;
                return; 
            }

            IsInitialized = false;
        }

        public override void Return(GameObject gameObject)
        {
            _isCanShoot = true;
            base.Return(gameObject);
        }

        private protected override void Attack()
        {
            base.Attack();

            _isCanShoot = false;
            _cross.gameObject.SetActive(true);
            _cross.StartAimWarning();
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/AimCross/SniperShot");
        }

        private void OnShoot()
        {
            AudioPlayer.Play();
        }
    }
}