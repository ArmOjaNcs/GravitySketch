using Assets.Sources.PlayerScripts;
using UnityEngine;
using Assets.Sources.Audio;
using Assets.Sources.Utils;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class EnemyAttackZone : EnemyZone, IEnemyAttack
    {
        private AudioPlayerSpawner _audioPlayerSpawner;

        private protected AudioClip AudioClip;
        private protected AudioPlayer AudioPlayer;
        private protected Transform FirePoint;
        private protected float CurrentTime;
        private protected float AttackRate;
        private protected bool IsAttacking;

        private protected override void Awake()
        {
            base.Awake();
            SetAudioClip();
        }

        private protected virtual void Update()
        {
            if (IsInitialized == false || IsAttacking == false || IsPaused || Player == null)
                return;

            if (Player.gameObject.activeSelf == false)
                return;

            CurrentTime += Time.deltaTime;

            if (CurrentTime > AttackRate)
                Attack();
        }

        public virtual void Initialize(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner)
        {
            FirePoint = firePoint;
            AttackRate = config.AttackRate;
            _audioPlayerSpawner = audioPlayerSpawner;
        }

        public virtual void Return(GameObject gameObject) => gameObject.SetActive(false);

        private protected override void PlayerDetected(Collider playerCollider)
        {
            base.PlayerDetected(playerCollider);
            IsAttacking = true;
        }

        private protected virtual void Attack()
        {
            if (Player == null)
                return;

            AudioPlayer = GetAudioPlayer();
            AudioPlayer.SetAudioClip(AudioClip);
            CurrentTime = 0;
        }
       
        private protected override void PlayerLosed(Collider playerCollider)
        {
            IsAttacking = false;
            CurrentTime = 0;
        }

        private protected abstract void SetAudioClip();

        private protected AudioPlayer GetAudioPlayer()
        {
            return _audioPlayerSpawner.GetAudioPlayer(FirePoint.position, UserUtils.MixerGroupSound);
        }
    }
}