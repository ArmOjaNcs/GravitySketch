using System;
using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemyRetreatZone : EnemyAttackZone
    {
        [SerializeField] private RetreatBomb _bomb;

        private bool _isCanDrop;

        public event Action<bool> ShouldRetreat;

        public float ColliderRadius => Collider == null ? 0 : Collider.radius * transform.localScale.x;

        private void OnEnable()
        {
            _bomb.gameObject.SetActive(false);
            _isCanDrop = true;
        }

        private protected override void Update()
        {
            if (_isCanDrop == false || _bomb.IsInitialized == false)
                return;

            base.Update();
        }

        public override void InitFromConfig(
            EnemyAttackConfig config,
            Transform firePoint,
            AudioPlayerSpawner audioPlayerSpawner,
            PauseHandler pauseHandler)
        {
            base.InitFromConfig(config, firePoint, audioPlayerSpawner, pauseHandler);

            BomberConfig bomberConfig = config.SafeCast<BomberConfig>();

            if (bomberConfig != null)
            {
                _bomb = Instantiate(_bomb);
                _bomb.InitFromConfig(bomberConfig.BombConfig, this);
                _bomb.Init(pauseHandler);
                _bomb.gameObject.SetActive(false);
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
        }

        public override void Return(GameObject gameObject)
        {
            _isCanDrop = true;
            _bomb.gameObject.SetActive(false);
        }

        private protected override void Attack()
        {
            base.Attack();

            AudioPlayer?.Play();
            _isCanDrop = false;
            _bomb.transform.position = FirePoint.position;
            _bomb.gameObject.SetActive(true);
        }

        private protected override void PlayerDetected()
        {
            base.PlayerDetected();
            ShouldRetreat?.Invoke(true);
        }

        private protected override void PlayerLosed()
        {
            base.PlayerLosed();
            ShouldRetreat?.Invoke(false);
        }

        private protected override void SetAudioClip()
        {
            AudioClip = Resources.Load<AudioClip>("Audio/Sounds/Bomb/GrenadeLauncher");
        }
    }
}