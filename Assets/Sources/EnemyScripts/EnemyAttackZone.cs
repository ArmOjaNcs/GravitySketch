using Assets.Sources.PlayerScripts;
using UnityEngine;
using Assets.Sources.Audio;
using Assets.Sources.Utils;
using Assets.Sources.Pause;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class EnemyAttackZone : EnemyZone, IEnemyAttack
    {
        private AudioPlayerSpawner _audioPlayerSpawner;

        private protected PauseHandler PauseHandler;
        private protected AudioClip AudioClip;
        private protected AudioPlayer AudioPlayer;
        private protected Transform FirePoint;
        private protected float CurrentTime;
        private protected float AttackRate;
        private protected bool IsAttacking;

        private protected virtual void Update()
        {
            if (IsActivated() == false)
                return;

            if (Player.gameObject.activeSelf == false)
                return;

            CurrentTime += Time.deltaTime;

            if (CurrentTime > AttackRate)
                Attack();
        }

        public virtual void InitFromConfig(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner, PauseHandler pauseHandler)
        {
            FirePoint = firePoint;
            AttackRate = config.AttackRate;
            _audioPlayerSpawner = audioPlayerSpawner;
            Init(pauseHandler);
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            SetAudioClip();
            PauseHandler = pauseHandler;
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

        private protected bool IsActivated()
        {
            return IsInitialized && IsAttacking && IsPaused == false && Player != null;
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